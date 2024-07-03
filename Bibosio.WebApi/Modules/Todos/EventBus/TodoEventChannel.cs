using Bibosio.WebApi.Common;
using Bibosio.WebApi.Modules.Todos.Interfaces;

namespace Bibosio.WebApi.Modules.Todos.EventBus
{
    internal sealed class TodoEventChannel : EventChannel, ITodoEventChannel
    { }
}
