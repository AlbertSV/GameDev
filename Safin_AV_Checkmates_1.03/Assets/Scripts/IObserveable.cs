using System;
using System.Threading.Tasks;


namespace Checks
{
    public interface IObserveable
    {
        public Task Serialize(string input);

        public event Action<(int, int)> NextStepReady;
        public event Action<(int, int)> NextStepReadyClick;
    }
}