namespace TDDMicroExercises.TurnTicketDispenser
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;

    public class TicketDispenserTests
    {
        [TestCase]
        public void GetOneTicketFromDispenser()
        {
            var dispenser = new TicketDispenser();

            var ticket = dispenser.GetTurnTicket();

            Assert.NotNull(ticket);
        }

        [TestCase]
        public void NoDuplicateTicketsFromOneMachine()
        {
            var dispenser = new TicketDispenser();
            var tickets = new HashSet<int>();
            for (int i = 0; i < 1000000; i++)
            {
                var ticket = dispenser.GetTurnTicket();

                Assert.False(tickets.Contains(ticket.TurnNumber));
                tickets.Add(ticket.TurnNumber);
            }
        }

        [TestCase]
        public void NoDuplicateTicketsFromMultipleMachines()
        {
            var dispensers = new List<TicketDispenser>{ new TicketDispenser(), new TicketDispenser(), new TicketDispenser() };
            var tickets = new HashSet<int>();
            for (int i = 0; i < 3333333; i++)
            {
                foreach (var dispenser in dispensers)
                {
                    var ticket = dispenser.GetTurnTicket();

                    Assert.False(tickets.Contains(ticket.TurnNumber));
                    tickets.Add(ticket.TurnNumber);
                }
            }
        }

        [TestCase]
        public void NoDuplicateTicketsFromMultipleMachinesWhenCalledInParallell()
        {
            var dispensers = new List<TicketDispenser> { new TicketDispenser(), new TicketDispenser(), new TicketDispenser() };
            var tickets = new HashSet<int>();
            for (int i = 0; i < 3333333; i++)
            {
                var tasks = new List<Task<TurnTicket>>();
                foreach (var dispenser in dispensers)
                {
                    tasks.Add(Task.Run(() => { return dispenser.GetTurnTicket(); }));
                }

                var completedTasks = Task.WhenAll(tasks).Result;

                foreach (var completedTask in completedTasks)
                {
                    Assert.False(tickets.Contains(completedTask.TurnNumber), $"Ticket number {completedTask.TurnNumber} has already been issued");
                    tickets.Add(completedTask.TurnNumber);
                }
            }
        }
    }
}
