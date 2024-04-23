# Scandoc.LocalCaching

Библиотека для работы с неперсистентными локальными кэшами.

## Основы

Базовая абстракция, представляющая локальный кэш:

```csharp
public interface ILocalCache<TData>
{
    ValueTask<TData> GetAsync(CancellationToken token);
    TData Get();
    void Reset();
}

public abstract class LocalCacheBase<TData> : ILocalCache<TData>
{
    public async ValueTask<TData> GetAsync(CancellationToken token) { ... }

    public TData Get() { ... }

    public void Reset() { ... }

    protected abstract Task<TData> FactoryAsync();
}
```

TData - объект, представляющий кэшируемые данные.

Методы Get и GetAsync позволяют синхронно или асинхронно получить TData. 

Если объект TData уже запрашивался ранее, то он будет получен из кэша.

В противном случае TData будет получен из имплементации `FactoryAsync()` и сохранен в кэше.

Применить кэш можно одним из двух способов:
- Имплементировать `LocalCacheBase<TData>`, передать необходимые зависимости в конструктор и переопределить `FactoryAsync()`
- Использовать готовую имплементацию `LocalCache<TData>`, принимающую в конструктор лямбду, которая будет вызвана в методе `FactoryAsync()`

### Сброс по событию

Чтобы кэш сбрасывался по событию, он должен быть зарегистрирован с помощью метода расширения `AddLocalCaching` с указанием ключа кэша и названия системы. Время жизни кэшей зарегистрированных таким образом - singleton.

Пример:
```csharp
serviceCollection.AddLocalCaching("your-system-name", redisConnectionMultiplexer, builder => builder
    .AddCache<ILocalCollectionCache<Banana>, CachedBananas>("Bananas")
    .AddCache<ILocalCollectionCache<Apple>>("Apples", provider => new LocalCollectionCache<Apple>(async () => await ...)));
```

Кэш из примера по ключу `"Bananas"` можно сбросить отправкой сообщения `"Bananas"` в канал `"LocalCacheReset"` redis pub/sub - это сбросит кэш по данному ключу во ВСЕХ приложениях.

По ключу `"Everything"` сбрасываются ВСЕ зарегистрированные через `AddLocalCaching` кэши.