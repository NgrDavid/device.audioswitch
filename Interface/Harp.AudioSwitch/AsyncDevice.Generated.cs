using Bonsai.Harp;
using System.Threading.Tasks;

namespace Harp.AudioSwitch
{
    /// <inheritdoc/>
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the asynchronous API to configure and interface
        /// with AudioSwitch devices on the specified serial port.
        /// </summary>
        /// <param name="portName">
        /// The name of the serial port used to communicate with the Harp device.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous initialization operation. The value of
        /// the <see cref="Task{TResult}.Result"/> parameter contains a new instance of
        /// the <see cref="AsyncDevice"/> class.
        /// </returns>
        public static async Task<AsyncDevice> CreateAsync(string portName)
        {
            var device = new AsyncDevice(portName);
            var whoAmI = await device.ReadWhoAmIAsync();
            if (whoAmI != Device.WhoAmI)
            {
                var errorMessage = string.Format(
                    "The device ID {1} on {0} was unexpected. Check whether a AudioSwitch device is connected to the specified serial port.",
                    portName, whoAmI);
                throw new HarpException(errorMessage);
            }

            return device;
        }
    }

    /// <summary>
    /// Represents an asynchronous API to configure and interface with AudioSwitch devices.
    /// </summary>
    public partial class AsyncDevice : Bonsai.Harp.AsyncDevice
    {
        internal AsyncDevice(string portName)
            : base(portName)
        {
        }

        /// <summary>
        /// Asynchronously reads the contents of the ControlMode register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ControlSource> ReadControlModeAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ControlMode.Address));
            return ControlMode.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ControlMode register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ControlSource>> ReadTimestampedControlModeAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ControlMode.Address));
            return ControlMode.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ControlMode register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteControlModeAsync(ControlSource value)
        {
            var request = ControlMode.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableChannels register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadEnableChannelsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(EnableChannels.Address));
            return EnableChannels.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableChannels register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedEnableChannelsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(EnableChannels.Address));
            return EnableChannels.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableChannels register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableChannelsAsync(ushort value)
        {
            var request = EnableChannels.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DigitalInputsState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalInputs> ReadDigitalInputsStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalInputsState.Address));
            return DigitalInputsState.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DigitalInputsState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalInputs>> ReadTimestampedDigitalInputsStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalInputsState.Address));
            return DigitalInputsState.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DO0State register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalState> ReadDO0StateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO0State.Address));
            return DO0State.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DO0State register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalState>> ReadTimestampedDO0StateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO0State.Address));
            return DO0State.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DO0State register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDO0StateAsync(DigitalState value)
        {
            var request = DO0State.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DI4Trigger register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DI4TriggerConfig> ReadDI4TriggerAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DI4Trigger.Address));
            return DI4Trigger.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DI4Trigger register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DI4TriggerConfig>> ReadTimestampedDI4TriggerAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DI4Trigger.Address));
            return DI4Trigger.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DI4Trigger register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDI4TriggerAsync(DI4TriggerConfig value)
        {
            var request = DI4Trigger.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DO0Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DO0SyncConfig> ReadDO0SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO0Sync.Address));
            return DO0Sync.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DO0Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DO0SyncConfig>> ReadTimestampedDO0SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO0Sync.Address));
            return DO0Sync.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DO0Sync register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDO0SyncAsync(DO0SyncConfig value)
        {
            var request = DO0Sync.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableEvents register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<AudioSwitchEvents> ReadEnableEventsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableEvents.Address));
            return EnableEvents.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableEvents register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<AudioSwitchEvents>> ReadTimestampedEnableEventsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableEvents.Address));
            return EnableEvents.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableEvents register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableEventsAsync(AudioSwitchEvents value)
        {
            var request = EnableEvents.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }
    }
}
