using System;
using System.Linq;
using Injection;
using Nanos.Internals;
using Nanos.Lifetime;
using Serilog;

namespace Nanos.Config
{
    public class NanosConfigurer
    {
        readonly ILogger _logger = Log.ForContext<NanosConfigurer>();
        readonly Injectionist _injectionist = new Injectionist();

        public IDisposable Start()
        {
            InstallDefaults();

            var result = ResolveInstance();

            HandleDisposal(result);

            Initialize(result);

            _logger.Information("Nanos started");

            return result.Instance;
        }

        void InstallDefaults()
        {
            _logger.Verbose("Installing defaults");

            if (!_injectionist.Has<INanosInstance>())
            {
                _injectionist.Register<INanosInstance>(c => new DefaultNanosInstance());
            }
        }

        ResolutionResult<INanosInstance> ResolveInstance()
        {
            _logger.Verbose("Building instance");

            return _injectionist.Get<INanosInstance>();
        }

        void Initialize(ResolutionResult<INanosInstance> result)
        {
            _logger.Verbose("Initializing Nanos");

            foreach (var initializable in result.TrackedInstances.OfType<IInitializable>())
            {
                _logger.Verbose("Initializing {InitializableType}", initializable.GetType());

                initializable.Initialize();
            }
        }

        void HandleDisposal(ResolutionResult<INanosInstance> result)
        {
            if (result is INotifyDisposal notifier)
            {
                notifier.Disposed += () =>
                {
                    _logger.Verbose("Disposing Nanos");

                    foreach (var disposable in result.TrackedInstances.OfType<IDisposable>().Reverse())
                    {
                        _logger.Verbose("Disposing {DisposableType}", disposable.GetType());

                        disposable.Dispose();
                    }

                    _logger.Information("Nanos stopped");
                };
            }
        }
    }
}