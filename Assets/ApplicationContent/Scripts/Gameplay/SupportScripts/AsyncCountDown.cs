using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// <para>Time measurement units.</para>
/// </summary>
public enum TimeUnit
{
    DAYS = 86400000,
    HOURS = 3600000,
    MINUTES = 60000,
    SECONDS = 1000,
    MILLISECONDS = 1
}

/// <summary>
/// <para>Class that sets up and starts the async countdown.</para>
/// </summary>
public sealed class AsyncCountDown
{
    private readonly CancellationToken token;
    private TimeUnit type;
    private Action<long> callback;
    private double period;
    private long count;

    /// <summary>
    /// <para>Method for starting the setup.</para>
    /// </summary>
    /// <returns>the AsyncCountDown instance for setup</returns>
    public static AsyncCountDown Setup(CancellationToken token)
    {
        return new AsyncCountDown(token);
    }

    private AsyncCountDown(CancellationToken token)
    {
        this.token = token;
        this.callback = null;
        this.count = 0;
        this.type = TimeUnit.SECONDS;
        this.period = 1;
    }

    private AsyncCountDown self()
    {
        return this;
    }

    /// <param name="callback">the callback to be called when the counter ticks</param>
    /// <returns>self</returns>
    public AsyncCountDown Callback(Action<long> callback)
    {
        this.callback = callback;
        return self();
    }

    /// <param name="count">the number of counter ticks</param>
    /// <returns>self</returns>
    public AsyncCountDown Count(long count)
    {
        this.count = count;
        return self();
    }

    /// <param name="period">the period between counter ticks</param>
    /// <returns>self</returns>
    public AsyncCountDown PeriodBetweenCallback(double period)
    {
        this.period = period;
        return self();
    }

    /// <param name="period">the period between counter ticks</param>
    /// <param name="type">the <see cref="TimeUnit"/></param>
    /// <returns>self</returns>
    public AsyncCountDown PeriodBetweenCallback(double period, TimeUnit type)
    {
        this.period = period;
        this.type = type;
        return self();
    }

    /// <summary>
    /// Starts count down.
    /// </summary>
    public void Start()
    {
        new CountDownExecutor(this);
    }

    private class CountDownExecutor
    {
        private readonly CancellationToken token;
        private readonly Action<long> callback;
        private readonly TimeUnit type;
        private readonly double period;
        private long count;

        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="builder">the configured AsyncCountDown class</param>
        public CountDownExecutor(AsyncCountDown builder)
        {
            this.token = builder.token;
            this.callback = builder.callback;
            this.type = builder.type;
            this.period = builder.period;
            this.count = builder.count;

            Tick();
        }

        private void Tick()
        {
            count--;
            callback?.Invoke(count);
            NextTick();
        }

        private async void NextTick()
        {
            if (count > 0)
            {
                double awaitMilliseconds = this.period * (int)this.type;
                await Task.Delay(TimeSpan.FromMilliseconds(awaitMilliseconds), token);
                Tick();
            }
        }
    }
}