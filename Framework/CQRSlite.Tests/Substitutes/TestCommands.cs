using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain.Exception;

namespace CQRSlite.Tests.Substitutes
{
    public class TestAggregateDoSomething : ICommand
    {
        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
        public bool LongRunning { get; set; }
    }

    public class TestAggregateDoSomethingHandler : ICommandHandler<TestAggregateDoSomething> 
    {
        public async Task Handle(TestAggregateDoSomething message, CancellationToken token)
        {
            if (message.LongRunning)
                await Task.Delay(50, token);
            if(message.ExpectedVersion != TimesRun)
                throw new ConcurrencyException(message.Id);
            TimesRun++;
            Token = token;
        }

        public int TimesRun { get; set; }
        public CancellationToken Token { get; set; }
    }
	public class TestAggregateDoSomethingElseHandler : ICommandHandler<TestAggregateDoSomething>
    {
        public Task Handle(TestAggregateDoSomething message, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}