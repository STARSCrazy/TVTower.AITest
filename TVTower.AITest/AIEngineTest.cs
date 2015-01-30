using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLua;

namespace TVTower.AITest
{
    [TestClass]
    public class AIEngineTest
    {
        [TestMethod]
        [DeploymentItem( "TestFiles\\SLF.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\AIEngine.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\BudgetManager.lua", "res\\ai\\" )]
        [DeploymentItem( "TestFiles\\TestAIPlayer.lua", "res\\ai\\" )]
        public void SortTasksByInvestmentPrioTest()
        {
            using ( var lua = new Lua() )
            {
                LuaTestHelper.SetTVTLua( lua );

                lua.DoString( "budgetManager = BudgetManager()" );

                {
                    var result = lua.DoString( @"
                    local tasks = {}

                    local task2 = TestTask()
                    task2.name = 'task2'
                    task2.CurrentInvestmentPriority = 30
                    tasks[1] = task2

                    local task1 = TestTask()
                    task1.name = 'task1'
                    task1.CurrentInvestmentPriority = 50
                    tasks[2] = task1

                    local task4 = TestTask()
                    task4.name = 'task4'
                    task4.CurrentInvestmentPriority = 5
                    tasks[3] = task4

                    local task5 = TestTask()
                    task5.name = 'task5'
                    task5.CurrentInvestmentPriority = 60
                    tasks[5] = task5

                    local task3 = TestTask()
                    task3.name = 'task3'
                    task3.CurrentInvestmentPriority = 20
                    tasks[4] = task3

                    return SortTasksByInvestmentPrio(tasks)" );

                    var table = (LuaTable)result[0];
                    Assert.AreEqual( "task5", ( (LuaTable)table.GetObjectByIndex( 0 ) ).GetValueByKey( "name" ) );
                    Assert.AreEqual( "task1", ( (LuaTable)table.GetObjectByIndex( 1 ) ).GetValueByKey( "name" ) );
                    Assert.AreEqual( "task2", ( (LuaTable)table.GetObjectByIndex( 2 ) ).GetValueByKey( "name" ) );
                    Assert.AreEqual( "task3", ( (LuaTable)table.GetObjectByIndex( 3 ) ).GetValueByKey( "name" ) );
                    Assert.AreEqual( "task4", ( (LuaTable)table.GetObjectByIndex( 4 ) ).GetValueByKey( "name" ) );
                }
            }
        }
    }
}
