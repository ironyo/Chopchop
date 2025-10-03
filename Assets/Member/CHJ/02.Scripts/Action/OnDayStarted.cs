using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/OnDayStarted")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "OnDayStarted", message: "Minion Starts the Day", category: "Events", id: "7ad1a23e54c67b4135f8e0769a743c8d")]
public sealed partial class OnDayStarted : EventChannel { }

