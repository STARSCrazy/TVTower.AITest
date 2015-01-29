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
    }
}
