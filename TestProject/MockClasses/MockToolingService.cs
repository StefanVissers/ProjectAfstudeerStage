using Frontend.Models;
using Frontend.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.MockClasses
{
    class MockToolingService : IToolingService
    {
        public CommandResult Execute(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
