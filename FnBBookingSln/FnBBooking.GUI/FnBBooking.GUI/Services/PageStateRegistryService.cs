using System;
using System.Collections.Concurrent;

namespace FnBBooking.GUI.Services
{
    /// <summary>
    /// Service Scoped quản trị trạng thái (filter, sort, paging) cho tất cả màn hình danh sách trong suốt phiên làm việc của User.
    /// Giúp bảo toàn state khi điều hướng Add/Edit hoặc chuyển qua lại giữa các màn hình.
    /// </summary>
    public interface IPageStateRegistryService
    {
        MasterDataPageState GetOrCreateState(string pageKey);
        void SetState(string pageKey, MasterDataPageState state);
        void ResetState(string pageKey);
        T GetCustomState<T>(string pageKey) where T : class, new();
        void SetCustomState<T>(string pageKey, T state);
    }

    public class PageStateRegistryService : IPageStateRegistryService
    {
        private readonly ConcurrentDictionary<string, MasterDataPageState> _masterDataStates = new(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, object> _customStates = new(StringComparer.OrdinalIgnoreCase);

        public MasterDataPageState GetOrCreateState(string pageKey)
        {
            if (string.IsNullOrWhiteSpace(pageKey))
                throw new ArgumentNullException(nameof(pageKey));

            return _masterDataStates.GetOrAdd(pageKey, _ => new MasterDataPageState());
        }

        public void SetState(string pageKey, MasterDataPageState state)
        {
            if (string.IsNullOrWhiteSpace(pageKey)) return;
            state.IsInitialized = true;
            _masterDataStates[pageKey] = state;
        }

        public void ResetState(string pageKey)
        {
            if (string.IsNullOrWhiteSpace(pageKey)) return;
            _masterDataStates.TryRemove(pageKey, out _);
            _customStates.TryRemove(pageKey, out _);
        }

        public T GetCustomState<T>(string pageKey) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(pageKey))
                throw new ArgumentNullException(nameof(pageKey));

            if (_customStates.TryGetValue(pageKey, out var obj) && obj is T typedObj)
            {
                return typedObj;
            }

            var newObj = new T();
            _customStates[pageKey] = newObj;
            return newObj;
        }

        public void SetCustomState<T>(string pageKey, T state)
        {
            if (string.IsNullOrWhiteSpace(pageKey)) return;
            _customStates[pageKey] = state;
        }
    }
}

