using Reflex.Core;
using UnityEngine;

namespace DI
{
    public class ItemsScreenInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton(new DataServerMock(), typeof(DataServerMock), typeof(IDataServer));
        }
    }
}