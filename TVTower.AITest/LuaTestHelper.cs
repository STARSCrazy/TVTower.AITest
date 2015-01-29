using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua;
using TVTower.AITest.TVTowerMock;

namespace TVTower.AITest
{
	public static class LuaTestHelper
	{
        public static void SetTVTLua( Lua lua )
		{
			lua.DoFile( "res\\ai\\SLF.lua" );
			lua.DoFile( "res\\ai\\AIEngine.lua" );
			lua.DoFile( "res\\ai\\BudgetManager.lua" );
			lua.DoFile( "res\\ai\\TestAIPlayer.lua" );

			var player = new Player();
			var tvt = new TVT();
			var worldTime = new WorldTime();

			lua.NewTable( "MY" );
			lua.RegisterFunction( "MY.GetMoney", player, typeof( Player ).GetMethod( "GetMoney" ) );

			lua.NewTable( "TVT" );
			lua.RegisterFunction( "TVT.addToLog", tvt, typeof( TVT ).GetMethod( "AddToLog" ) );
			lua.RegisterFunction( "TVT.GetMillisecs", tvt, typeof( TVT ).GetMethod( "GetMillisecs" ) );

			lua.NewTable( "WorldTime" );
			lua.RegisterFunction( "WorldTime.GetDaysRun", worldTime, typeof( WorldTime ).GetMethod( "GetDaysRun" ) );
			lua.RegisterFunction( "WorldTime.GetTimeGoneAsMinute", worldTime, typeof( WorldTime ).GetMethod( "GetTimeGoneAsMinute" ) );

			lua.DoString( @"globalPlayer = TestAIPlayer()
					globalPlayer:initialize()
					_G['globalPlayer'] = globalPlayer" );   
		}
	}
}
