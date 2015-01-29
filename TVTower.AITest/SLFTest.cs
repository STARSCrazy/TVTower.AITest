using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLua;

namespace TVTower.AITest
{
    [TestClass]
    public class SLFTest
    {
        [TestMethod]
        [DeploymentItem( "TestFiles\\SLF.lua" )]
        public void SLFTest1()
        {
            using ( var lua = new Lua() )
            {
                lua.DoFile( "SLF.lua" );

                Assert.AreEqual( "abc       ", lua.DoString( "return string.left(\"abc\", 10, true)" ).FirstOrDefault() );
                Assert.AreEqual( "       abc", lua.DoString( "return string.right(\"abc\", 10, true)" ).FirstOrDefault() );
            }
        }
    }
}
