using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.AudioSwitch
{
    /// <summary>
    /// Generates events and processes commands for the AudioSwitch device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the AudioSwitch device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="AudioSwitch"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 1248;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(AudioSwitch);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(ControlMode) },
            { 33, typeof(EnableChannels) },
            { 34, typeof(DigitalInputState) },
            { 35, typeof(DO0State) },
            { 36, typeof(Reserved0) },
            { 37, typeof(DI4Trigger) },
            { 38, typeof(DO0Sync) },
            { 39, typeof(EnableEvents) }
        };

        /// <summary>
        /// Gets the contents of the metadata file describing the <see cref="AudioSwitch"/>
        /// device registers.
        /// </summary>
        public static readonly string Metadata = GetDeviceMetadata();

        static string GetDeviceMetadata()
        {
            var deviceType = typeof(Device);
            using var metadataStream = deviceType.Assembly.GetManifestResourceStream($"{deviceType.Namespace}.device.yml");
            using var streamReader = new System.IO.StreamReader(metadataStream);
            return streamReader.ReadToEnd();
        }
    }

    /// <summary>
    /// Represents an operator that returns the contents of the metadata file
    /// describing the <see cref="AudioSwitch"/> device registers.
    /// </summary>
    [Description("Returns the contents of the metadata file describing the AudioSwitch device registers.")]
    public partial class GetMetadata : Source<string>
    {
        /// <summary>
        /// Returns an observable sequence with the contents of the metadata file
        /// describing the <see cref="AudioSwitch"/> device registers.
        /// </summary>
        /// <returns>
        /// A sequence with a single <see cref="string"/> object representing the
        /// contents of the metadata file.
        /// </returns>
        public override IObservable<string> Generate()
        {
            return Observable.Return(Device.Metadata);
        }
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="AudioSwitch"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of AudioSwitch messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="AudioSwitch"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="AudioSwitch"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="AudioSwitch"/> device.
    /// </summary>
    /// <seealso cref="ControlMode"/>
    /// <seealso cref="EnableChannels"/>
    /// <seealso cref="DigitalInputState"/>
    /// <seealso cref="DO0State"/>
    /// <seealso cref="DI4Trigger"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(ControlMode))]
    [XmlInclude(typeof(EnableChannels))]
    [XmlInclude(typeof(DigitalInputState))]
    [XmlInclude(typeof(DO0State))]
    [XmlInclude(typeof(DI4Trigger))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(EnableEvents))]
    [Description("Filters register-specific messages reported by the AudioSwitch device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new ControlMode();
        }

        string INamedElement.Name
        {
            get => $"{nameof(AudioSwitch)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the AudioSwitch device.
    /// </summary>
    /// <seealso cref="ControlMode"/>
    /// <seealso cref="EnableChannels"/>
    /// <seealso cref="DigitalInputState"/>
    /// <seealso cref="DO0State"/>
    /// <seealso cref="DI4Trigger"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(ControlMode))]
    [XmlInclude(typeof(EnableChannels))]
    [XmlInclude(typeof(DigitalInputState))]
    [XmlInclude(typeof(DO0State))]
    [XmlInclude(typeof(DI4Trigger))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(EnableEvents))]
    [XmlInclude(typeof(TimestampedControlMode))]
    [XmlInclude(typeof(TimestampedEnableChannels))]
    [XmlInclude(typeof(TimestampedDigitalInputState))]
    [XmlInclude(typeof(TimestampedDO0State))]
    [XmlInclude(typeof(TimestampedDI4Trigger))]
    [XmlInclude(typeof(TimestampedDO0Sync))]
    [XmlInclude(typeof(TimestampedEnableEvents))]
    [Description("Filters and selects specific messages reported by the AudioSwitch device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new ControlMode();
        }

        string INamedElement.Name => $"{nameof(AudioSwitch)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// AudioSwitch register messages.
    /// </summary>
    /// <seealso cref="ControlMode"/>
    /// <seealso cref="EnableChannels"/>
    /// <seealso cref="DigitalInputState"/>
    /// <seealso cref="DO0State"/>
    /// <seealso cref="DI4Trigger"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(ControlMode))]
    [XmlInclude(typeof(EnableChannels))]
    [XmlInclude(typeof(DigitalInputState))]
    [XmlInclude(typeof(DO0State))]
    [XmlInclude(typeof(DI4Trigger))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(EnableEvents))]
    [Description("Formats a sequence of values as specific AudioSwitch register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new ControlMode();
        }

        string INamedElement.Name => $"{nameof(AudioSwitch)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that configures the source to enable the board channels.
    /// </summary>
    [Description("Configures the source to enable the board channels.")]
    public partial class ControlMode
    {
        /// <summary>
        /// Represents the address of the <see cref="ControlMode"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="ControlMode"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ControlMode"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ControlMode"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ControlSource GetPayload(HarpMessage message)
        {
            return (ControlSource)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ControlMode"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ControlSource> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ControlSource)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ControlMode"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ControlMode"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ControlSource value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ControlMode"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ControlMode"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ControlSource value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ControlMode register.
    /// </summary>
    /// <seealso cref="ControlMode"/>
    [Description("Filters and selects timestamped messages from the ControlMode register.")]
    public partial class TimestampedControlMode
    {
        /// <summary>
        /// Represents the address of the <see cref="ControlMode"/> register. This field is constant.
        /// </summary>
        public const int Address = ControlMode.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ControlMode"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ControlSource> GetPayload(HarpMessage message)
        {
            return ControlMode.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.
    /// </summary>
    [Description("Enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.")]
    public partial class EnableChannels
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableChannels"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableChannels"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="EnableChannels"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableChannels"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static AudioChannels GetPayload(HarpMessage message)
        {
            return (AudioChannels)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableChannels"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AudioChannels> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((AudioChannels)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableChannels"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableChannels"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, AudioChannels value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableChannels"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableChannels"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, AudioChannels value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableChannels register.
    /// </summary>
    /// <seealso cref="EnableChannels"/>
    [Description("Filters and selects timestamped messages from the EnableChannels register.")]
    public partial class TimestampedEnableChannels
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableChannels"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableChannels.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableChannels"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AudioChannels> GetPayload(HarpMessage message)
        {
            return EnableChannels.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.
    /// </summary>
    [Description("State of the digital input pins. An event will be emitted when the value of any digital input pin changes.")]
    public partial class DigitalInputState
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalInputState"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalInputState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalInputState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalInputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalInputs GetPayload(HarpMessage message)
        {
            return (DigitalInputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalInputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalInputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalInputState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalInputState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalInputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalInputState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalInputState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalInputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalInputState register.
    /// </summary>
    /// <seealso cref="DigitalInputState"/>
    [Description("Filters and selects timestamped messages from the DigitalInputState register.")]
    public partial class TimestampedDigitalInputState
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalInputState"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalInputState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalInputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputs> GetPayload(HarpMessage message)
        {
            return DigitalInputState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that status of the digital output pin 0.
    /// </summary>
    [Description("Status of the digital output pin 0.")]
    public partial class DO0State
    {
        /// <summary>
        /// Represents the address of the <see cref="DO0State"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="DO0State"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DO0State"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DO0State"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EnableFlag GetPayload(HarpMessage message)
        {
            return (EnableFlag)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DO0State"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EnableFlag)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DO0State"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO0State"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DO0State"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO0State"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DO0State register.
    /// </summary>
    /// <seealso cref="DO0State"/>
    [Description("Filters and selects timestamped messages from the DO0State register.")]
    public partial class TimestampedDO0State
    {
        /// <summary>
        /// Represents the address of the <see cref="DO0State"/> register. This field is constant.
        /// </summary>
        public const int Address = DO0State.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DO0State"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetPayload(HarpMessage message)
        {
            return DO0State.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved0
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved0"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that configuration of the digital input pin 4 functionality.
    /// </summary>
    [Description("Configuration of the digital input pin 4 functionality.")]
    public partial class DI4Trigger
    {
        /// <summary>
        /// Represents the address of the <see cref="DI4Trigger"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="DI4Trigger"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DI4Trigger"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DI4Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DI4TriggerConfig GetPayload(HarpMessage message)
        {
            return (DI4TriggerConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DI4Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DI4TriggerConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DI4TriggerConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DI4Trigger"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DI4Trigger"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DI4TriggerConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DI4Trigger"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DI4Trigger"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DI4TriggerConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DI4Trigger register.
    /// </summary>
    /// <seealso cref="DI4Trigger"/>
    [Description("Filters and selects timestamped messages from the DI4Trigger register.")]
    public partial class TimestampedDI4Trigger
    {
        /// <summary>
        /// Represents the address of the <see cref="DI4Trigger"/> register. This field is constant.
        /// </summary>
        public const int Address = DI4Trigger.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DI4Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DI4TriggerConfig> GetPayload(HarpMessage message)
        {
            return DI4Trigger.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital output pin 0 functionality.
    /// </summary>
    [Description("Configuration of the digital output pin 0 functionality.")]
    public partial class DO0Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DO0SyncConfig GetPayload(HarpMessage message)
        {
            return (DO0SyncConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO0SyncConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DO0SyncConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DO0Sync"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO0Sync"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DO0SyncConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DO0Sync"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO0Sync"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DO0SyncConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DO0Sync register.
    /// </summary>
    /// <seealso cref="DO0Sync"/>
    [Description("Filters and selects timestamped messages from the DO0Sync register.")]
    public partial class TimestampedDO0Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = DO0Sync.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO0SyncConfig> GetPayload(HarpMessage message)
        {
            return DO0Sync.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the active events in the device.
    /// </summary>
    [Description("Specifies the active events in the device.")]
    public partial class EnableEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static AudioSwitchEvents GetPayload(HarpMessage message)
        {
            return (AudioSwitchEvents)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AudioSwitchEvents> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((AudioSwitchEvents)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableEvents"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableEvents"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, AudioSwitchEvents value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableEvents"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableEvents"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, AudioSwitchEvents value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableEvents register.
    /// </summary>
    /// <seealso cref="EnableEvents"/>
    [Description("Filters and selects timestamped messages from the EnableEvents register.")]
    public partial class TimestampedEnableEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableEvents.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AudioSwitchEvents> GetPayload(HarpMessage message)
        {
            return EnableEvents.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// AudioSwitch device.
    /// </summary>
    /// <seealso cref="CreateControlModePayload"/>
    /// <seealso cref="CreateEnableChannelsPayload"/>
    /// <seealso cref="CreateDigitalInputStatePayload"/>
    /// <seealso cref="CreateDO0StatePayload"/>
    /// <seealso cref="CreateDI4TriggerPayload"/>
    /// <seealso cref="CreateDO0SyncPayload"/>
    /// <seealso cref="CreateEnableEventsPayload"/>
    [XmlInclude(typeof(CreateControlModePayload))]
    [XmlInclude(typeof(CreateEnableChannelsPayload))]
    [XmlInclude(typeof(CreateDigitalInputStatePayload))]
    [XmlInclude(typeof(CreateDO0StatePayload))]
    [XmlInclude(typeof(CreateDI4TriggerPayload))]
    [XmlInclude(typeof(CreateDO0SyncPayload))]
    [XmlInclude(typeof(CreateEnableEventsPayload))]
    [XmlInclude(typeof(CreateTimestampedControlModePayload))]
    [XmlInclude(typeof(CreateTimestampedEnableChannelsPayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalInputStatePayload))]
    [XmlInclude(typeof(CreateTimestampedDO0StatePayload))]
    [XmlInclude(typeof(CreateTimestampedDI4TriggerPayload))]
    [XmlInclude(typeof(CreateTimestampedDO0SyncPayload))]
    [XmlInclude(typeof(CreateTimestampedEnableEventsPayload))]
    [Description("Creates standard message payloads for the AudioSwitch device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateControlModePayload();
        }

        string INamedElement.Name => $"{nameof(AudioSwitch)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configures the source to enable the board channels.
    /// </summary>
    [DisplayName("ControlModePayload")]
    [Description("Creates a message payload that configures the source to enable the board channels.")]
    public partial class CreateControlModePayload
    {
        /// <summary>
        /// Gets or sets the value that configures the source to enable the board channels.
        /// </summary>
        [Description("The value that configures the source to enable the board channels.")]
        public ControlSource ControlMode { get; set; }

        /// <summary>
        /// Creates a message payload for the ControlMode register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ControlSource GetPayload()
        {
            return ControlMode;
        }

        /// <summary>
        /// Creates a message that configures the source to enable the board channels.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ControlMode register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.AudioSwitch.ControlMode.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configures the source to enable the board channels.
    /// </summary>
    [DisplayName("TimestampedControlModePayload")]
    [Description("Creates a timestamped message payload that configures the source to enable the board channels.")]
    public partial class CreateTimestampedControlModePayload : CreateControlModePayload
    {
        /// <summary>
        /// Creates a timestamped message that configures the source to enable the board channels.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ControlMode register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.AudioSwitch.ControlMode.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.
    /// </summary>
    [DisplayName("EnableChannelsPayload")]
    [Description("Creates a message payload that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.")]
    public partial class CreateEnableChannelsPayload
    {
        /// <summary>
        /// Gets or sets the value that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.
        /// </summary>
        [Description("The value that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.")]
        public AudioChannels EnableChannels { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableChannels register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public AudioChannels GetPayload()
        {
            return EnableChannels;
        }

        /// <summary>
        /// Creates a message that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableChannels register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.AudioSwitch.EnableChannels.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.
    /// </summary>
    [DisplayName("TimestampedEnableChannelsPayload")]
    [Description("Creates a timestamped message payload that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.")]
    public partial class CreateTimestampedEnableChannelsPayload : CreateEnableChannelsPayload
    {
        /// <summary>
        /// Creates a timestamped message that enables the audio output channels using a bitmask format. An event will be emitted when any of the channels are enabled.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableChannels register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.AudioSwitch.EnableChannels.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.
    /// </summary>
    [DisplayName("DigitalInputStatePayload")]
    [Description("Creates a message payload that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.")]
    public partial class CreateDigitalInputStatePayload
    {
        /// <summary>
        /// Gets or sets the value that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.
        /// </summary>
        [Description("The value that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.")]
        public DigitalInputs DigitalInputState { get; set; }

        /// <summary>
        /// Creates a message payload for the DigitalInputState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalInputs GetPayload()
        {
            return DigitalInputState;
        }

        /// <summary>
        /// Creates a message that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DigitalInputState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.AudioSwitch.DigitalInputState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.
    /// </summary>
    [DisplayName("TimestampedDigitalInputStatePayload")]
    [Description("Creates a timestamped message payload that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.")]
    public partial class CreateTimestampedDigitalInputStatePayload : CreateDigitalInputStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that state of the digital input pins. An event will be emitted when the value of any digital input pin changes.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DigitalInputState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.AudioSwitch.DigitalInputState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that status of the digital output pin 0.
    /// </summary>
    [DisplayName("DO0StatePayload")]
    [Description("Creates a message payload that status of the digital output pin 0.")]
    public partial class CreateDO0StatePayload
    {
        /// <summary>
        /// Gets or sets the value that status of the digital output pin 0.
        /// </summary>
        [Description("The value that status of the digital output pin 0.")]
        public EnableFlag DO0State { get; set; }

        /// <summary>
        /// Creates a message payload for the DO0State register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EnableFlag GetPayload()
        {
            return DO0State;
        }

        /// <summary>
        /// Creates a message that status of the digital output pin 0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DO0State register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.AudioSwitch.DO0State.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that status of the digital output pin 0.
    /// </summary>
    [DisplayName("TimestampedDO0StatePayload")]
    [Description("Creates a timestamped message payload that status of the digital output pin 0.")]
    public partial class CreateTimestampedDO0StatePayload : CreateDO0StatePayload
    {
        /// <summary>
        /// Creates a timestamped message that status of the digital output pin 0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DO0State register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.AudioSwitch.DO0State.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital input pin 4 functionality.
    /// </summary>
    [DisplayName("DI4TriggerPayload")]
    [Description("Creates a message payload that configuration of the digital input pin 4 functionality.")]
    public partial class CreateDI4TriggerPayload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital input pin 4 functionality.
        /// </summary>
        [Description("The value that configuration of the digital input pin 4 functionality.")]
        public DI4TriggerConfig DI4Trigger { get; set; }

        /// <summary>
        /// Creates a message payload for the DI4Trigger register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DI4TriggerConfig GetPayload()
        {
            return DI4Trigger;
        }

        /// <summary>
        /// Creates a message that configuration of the digital input pin 4 functionality.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DI4Trigger register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.AudioSwitch.DI4Trigger.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital input pin 4 functionality.
    /// </summary>
    [DisplayName("TimestampedDI4TriggerPayload")]
    [Description("Creates a timestamped message payload that configuration of the digital input pin 4 functionality.")]
    public partial class CreateTimestampedDI4TriggerPayload : CreateDI4TriggerPayload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital input pin 4 functionality.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DI4Trigger register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.AudioSwitch.DI4Trigger.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital output pin 0 functionality.
    /// </summary>
    [DisplayName("DO0SyncPayload")]
    [Description("Creates a message payload that configuration of the digital output pin 0 functionality.")]
    public partial class CreateDO0SyncPayload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output pin 0 functionality.
        /// </summary>
        [Description("The value that configuration of the digital output pin 0 functionality.")]
        public DO0SyncConfig DO0Sync { get; set; }

        /// <summary>
        /// Creates a message payload for the DO0Sync register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DO0SyncConfig GetPayload()
        {
            return DO0Sync;
        }

        /// <summary>
        /// Creates a message that configuration of the digital output pin 0 functionality.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DO0Sync register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.AudioSwitch.DO0Sync.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital output pin 0 functionality.
    /// </summary>
    [DisplayName("TimestampedDO0SyncPayload")]
    [Description("Creates a timestamped message payload that configuration of the digital output pin 0 functionality.")]
    public partial class CreateTimestampedDO0SyncPayload : CreateDO0SyncPayload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital output pin 0 functionality.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DO0Sync register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.AudioSwitch.DO0Sync.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the active events in the device.
    /// </summary>
    [DisplayName("EnableEventsPayload")]
    [Description("Creates a message payload that specifies the active events in the device.")]
    public partial class CreateEnableEventsPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the active events in the device.
        /// </summary>
        [Description("The value that specifies the active events in the device.")]
        public AudioSwitchEvents EnableEvents { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableEvents register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public AudioSwitchEvents GetPayload()
        {
            return EnableEvents;
        }

        /// <summary>
        /// Creates a message that specifies the active events in the device.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableEvents register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.AudioSwitch.EnableEvents.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the active events in the device.
    /// </summary>
    [DisplayName("TimestampedEnableEventsPayload")]
    [Description("Creates a timestamped message payload that specifies the active events in the device.")]
    public partial class CreateTimestampedEnableEventsPayload : CreateEnableEventsPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the active events in the device.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableEvents register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.AudioSwitch.EnableEvents.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Specifies the available audio output channels.
    /// </summary>
    [Flags]
    public enum AudioChannels : ushort
    {
        None = 0x0,
        Channel0 = 0x1,
        Channel1 = 0x2,
        Channel2 = 0x4,
        Channel3 = 0x8,
        Channel4 = 0x10,
        Channel5 = 0x20,
        Channel6 = 0x40,
        Channel7 = 0x80,
        Channel8 = 0x100,
        Channel9 = 0x200,
        Channel10 = 0x400,
        Channel11 = 0x800,
        Channel12 = 0x1000,
        Channel13 = 0x2000,
        Channel14 = 0x4000,
        Channel15 = 0x8000
    }

    /// <summary>
    /// Specifies the state of the digital input pins.
    /// </summary>
    [Flags]
    public enum DigitalInputs : byte
    {
        None = 0x0,
        DI0 = 0x1,
        DI1 = 0x2,
        DI2 = 0x4,
        DI3 = 0x8,
        DI4 = 0x10
    }

    /// <summary>
    /// The events that can be enabled/disabled.
    /// </summary>
    [Flags]
    public enum AudioSwitchEvents : byte
    {
        None = 0x0,
        EnableChannels = 0x1,
        DigitalInputsState = 0x2
    }

    /// <summary>
    /// Available configurations to control the board channels (host computer or digital inputs).
    /// </summary>
    public enum ControlSource : byte
    {
        USB = 0,
        DigitalInputs = 1
    }

    /// <summary>
    /// Available configurations for DI4. Can be used as digital input or as the MSB of the switches address when the SourceControl is configured as DigitalInputs.
    /// </summary>
    public enum DI4TriggerConfig : byte
    {
        Input = 0,
        Address = 1
    }

    /// <summary>
    /// Available configurations when using DO0 pin to report firmware events.
    /// </summary>
    public enum DO0SyncConfig : byte
    {
        Output = 0,
        ToggleOnChannelChange = 1
    }
}
