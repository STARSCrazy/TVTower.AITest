using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLua;
using TVTower.AITest.TVTowerMock;

namespace TVTower.AITest
{
    [TestClass]
    public class BudgetTest
    {
        [TestMethod]
        [DeploymentItem( "TestFiles\\SLF.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\AIEngine.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\BudgetManager.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\TestAIPlayer.lua", "res\\ai\\" )]        
        public void SLFTest12()
        {
            using ( var lua = new Lua() )
            {
                LuaTestHelper.SetTVTLua( lua );

                lua.DoString( "_G['globalPlayer'].TaskList['TestTask1'].BudgetWeigth = 1" );
                lua.DoString( "_G['globalPlayer'].TaskList['TestTask2'].BudgetWeigth = 2" );

                Assert.AreEqual( (double)0, lua.DoString( "return _G['globalPlayer'].TaskList['TestTask1'].CurrentBudget" ).FirstOrDefault() );
                Assert.AreEqual( (double)0, lua.DoString( "return _G['globalPlayer'].TaskList['TestTask2'].CurrentBudget" ).FirstOrDefault() );

                lua.DoString( @"local budgetManager = BudgetManager()
                budgetManager:Initialize()
                budgetManager:CalculateBudget()" );

                Assert.AreEqual( (double)232000, lua.DoString( "return _G['globalPlayer'].TaskList['TestTask1'].CurrentBudget" ).FirstOrDefault() );
                Assert.AreEqual( (double)464000, lua.DoString( "return _G['globalPlayer'].TaskList['TestTask2'].CurrentBudget" ).FirstOrDefault() );
            }
        }

        [TestMethod]
        [DeploymentItem( "TestFiles\\SLF.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\AIEngine.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\BudgetManager.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\TestAIPlayer.lua", "res\\ai\\" )]
        public void InvestmentTest()
        {
            System.Diagnostics.Trace.WriteLine( "Start" );

            using ( var lua = new Lua() )
            {
                LuaTestHelper.SetTVTLua( lua );

                lua.DoString( "budgetManager = BudgetManager()");

                //1. Bedingung
                {
                    var result = lua.DoString( @"
                    budgetManager.InvestmentSavings = 300000
                    local task1 = TestTask()
                    task1.CurrentInvestmentPriority = 10
                    task1.BudgetWholeDay = 100000
                    task1.NeededInvestmentBudget = 400000
                    return budgetManager:IsTaskReadyForInvestment(task1, 1)" );

                    Assert.IsTrue( (bool)result.FirstOrDefault() );

                    result = lua.DoString( @"
                    local task2 = TestTask()
                    task2.CurrentInvestmentPriority = 10
                    task2.BudgetWholeDay = 100000
                    task2.NeededInvestmentBudget = 600000
                    return budgetManager:IsTaskReadyForInvestment(task2, 1)" );

                    Assert.IsFalse( (bool)result.FirstOrDefault() );
                }

                //2. Bedingung
                {
                    var result = lua.DoString( @"budgetManager = BudgetManager()
                    budgetManager.InvestmentSavings = 300000
                    local task1 = TestTask()
                    task1.CurrentInvestmentPriority = 10
                    task1.BudgetWholeDay = 100000
                    task1.NeededInvestmentBudget = 400000
                    return budgetManager:IsTaskReadyForInvestment(task1, 1), budgetManager:IsTaskReadyForInvestment(task1, 2), budgetManager:IsTaskReadyForInvestment(task1, 3)" );

                    Assert.IsTrue( (bool)result[0] );
                    Assert.IsFalse( (bool)result[1] );
                    Assert.IsFalse( (bool)result[2] );

                    result = lua.DoString( @"
                    local task2 = TestTask()
                    task2.CurrentInvestmentPriority = 25
                    task2.BudgetWholeDay = 100000
                    task2.NeededInvestmentBudget = 400000
                    return budgetManager:IsTaskReadyForInvestment(task2, 1), budgetManager:IsTaskReadyForInvestment(task2, 2), budgetManager:IsTaskReadyForInvestment(task2, 3)" );

                    Assert.IsTrue( (bool)result[0] );
                    Assert.IsTrue( (bool)result[1] );
                    Assert.IsFalse( (bool)result[2] );
                }

                //3. Bedingung
                {
                    var result = lua.DoString( @"budgetManager = BudgetManager()
                    budgetManager.InvestmentSavings = 300000
                    local bestTask1 = TestTask()
                    bestTask1.CurrentInvestmentPriority = 55
                    bestTask1.NeededInvestmentBudget = 1000000

                    local bestTask2 = TestTask()
                    bestTask2.CurrentInvestmentPriority = 65
                    bestTask2.NeededInvestmentBudget = 1000000

                    local task1 = TestTask()
                    task1.CurrentInvestmentPriority = 30
                    task1.BudgetWholeDay = 100000
                    task1.NeededInvestmentBudget = 400000
                    return budgetManager:IsTaskReadyForInvestment(task1, 1), budgetManager:IsTaskReadyForInvestment(task1, 2, bestTask1), budgetManager:IsTaskReadyForInvestment(task1, 3, bestTask2)" );

                    Assert.IsTrue( (bool)result[0] );
                    Assert.IsTrue( (bool)result[1] );
                    Assert.IsFalse( (bool)result[2] );
                }


                //4. Bedingung
                {
                    var result = lua.DoString( @"budgetManager = BudgetManager()
                    budgetManager.InvestmentSavings = 300000
                    local bestTask1 = TestTask()
                    bestTask1.CurrentInvestmentPriority = 50
                    bestTask1.NeededInvestmentBudget = 1000000

                    local bestTask2 = TestTask()
                    bestTask2.CurrentInvestmentPriority = 50
                    bestTask2.NeededInvestmentBudget = 310000

                    local task1 = TestTask()
                    task1.CurrentInvestmentPriority = 30
                    task1.BudgetWholeDay = 100000
                    task1.NeededInvestmentBudget = 400000
                    return budgetManager:IsTaskReadyForInvestment(task1, 2, bestTask1), budgetManager:IsTaskReadyForInvestment(task1, 3, bestTask2)" );

                    Assert.IsTrue( (bool)result[0] );
                    Assert.IsFalse( (bool)result[1] );
                }
            }
        }

        [TestMethod]
        [DeploymentItem( "TestFiles\\SLF.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\AIEngine.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\BudgetManager.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\TestAIPlayer.lua", "res\\ai\\" )]
        public void GetTaskForInvestmentTest()
        {
            System.Diagnostics.Trace.WriteLine( "Start" );

            using ( var lua = new Lua() )
            {
                LuaTestHelper.SetTVTLua( lua );

                lua.DoString( "budgetManager = BudgetManager()" );
            
                {
                    var result = lua.DoString( @"budgetManager = BudgetManager()
                    budgetManager.InvestmentSavings = 300000
                    
                    local tasks = {}

                    local task1 = TestTask()
                    task1.name = 'task1'
                    task1.CurrentInvestmentPriority = 50
                    task1.BudgetWholeDay = 100000
                    task1.NeededInvestmentBudget = 500000
                    tasks[2] = task1

                    local task2 = TestTask()
                    task2.name = 'task2'
                    task2.CurrentInvestmentPriority = 30
                    task2.BudgetWholeDay = 100000
                    task2.NeededInvestmentBudget = 200000
                    tasks[1] = task2

                    local task3 = TestTask()
                    task3.name = 'task3'
                    task3.CurrentInvestmentPriority = 20
                    task3.BudgetWholeDay = 100000
                    task3.NeededInvestmentBudget = 150000
                    tasks[4] = task3

                    local task4 = TestTask()
                    task4.name = 'task4'
                    task4.CurrentInvestmentPriority = 5
                    task4.BudgetWholeDay = 100000
                    task4.NeededInvestmentBudget = 150000
                    tasks[3] = task4

                    return budgetManager:GetTaskForInvestment(tasks)" );
                    var table = (LuaTable)result[0];
                    Assert.AreEqual( "task2", table.GetValueByKey( "name" ) );
                }

                //Platz 1	90	0	500000
                //Platz 2	80	0	400000
                //Platz 3	70	150000	150000

                {
                    var result = lua.DoString( @"budgetManager = BudgetManager()
                    budgetManager.InvestmentSavings = 300000
                    
                    local tasks = {}

                    local task1 = TestTask()
                    task1.name = 'task1'
                    task1.CurrentInvestmentPriority = 90
                    task1.BudgetWholeDay = 0
                    task1.NeededInvestmentBudget = 500000
                    tasks[3] = task1

                    local task2 = TestTask()
                    task2.name = 'task2'
                    task2.CurrentInvestmentPriority = 80
                    task2.BudgetWholeDay = 0
                    task2.NeededInvestmentBudget = 400000
                    tasks[1] = task2

                    local task3 = TestTask()
                    task3.name = 'task3'
                    task3.CurrentInvestmentPriority = 70
                    task3.BudgetWholeDay = 150000
                    task3.NeededInvestmentBudget = 200000
                    tasks[0] = task3

                    return budgetManager:GetTaskForInvestment(tasks)" );
                    var table = (LuaTable)result[0];
                    Assert.AreEqual( "task3", table.GetValueByKey( "name" ) );
                }
            }
        }

        [TestMethod]
        [DeploymentItem( "TestFiles\\SLF.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\AIEngine.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\BudgetManager.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\TestAIPlayer.lua", "res\\ai\\" )]
        public void InvestmentTestCases()
        {
            System.Diagnostics.Trace.WriteLine( "Start" );

            using ( var lua = new Lua() )
            {
                LuaTestHelper.SetTVTLua( lua );

                lua.DoString( "budgetManager = BudgetManager()" );
            
                //4. Bedingung
                {
                    var result = lua.DoString( @"budgetManager = BudgetManager()
                    budgetManager.InvestmentSavings = 300000
                    local bestTask1 = TestTask()
                    bestTask1.CurrentInvestmentPriority = 50
                    bestTask1.NeededInvestmentBudget = 1000000

                    local bestTask2 = TestTask()
                    bestTask2.CurrentInvestmentPriority = 50
                    bestTask2.NeededInvestmentBudget = 310000

                    local task1 = TestTask()
                    task1.CurrentInvestmentPriority = 30
                    task1.BudgetWholeDay = 100000
                    task1.NeededInvestmentBudget = 400000
                    return budgetManager:IsTaskReadyForInvestment(task1, 2, bestTask1), budgetManager:IsTaskReadyForInvestment(task1, 3, bestTask2)" );

                    Assert.IsTrue( (bool)result[0] );
                    Assert.IsFalse( (bool)result[1] );
                }
            }
        }
    }
}
