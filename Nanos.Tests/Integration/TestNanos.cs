using System;
using System.Threading.Tasks;
using Nanos.Config;
using NUnit.Framework;

namespace Nanos.Tests.Integration
{
    [TestFixture]
    public class TestNanos : FixtureBase
    {
        IDisposable _nanos;

        protected override void SetUp()
        {
            _nanos = Configure.Nanos().Start();

            Using(_nanos);
        }

        [Test]
        public async Task CanStartAndStopIt()
        {
            
        }
    }
}