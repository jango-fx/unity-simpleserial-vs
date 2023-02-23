using Unity.VisualScripting;
using UnityEngine;

namespace ƒx.SimpleSerial
{
    [UnitTitle("Serial Send String")]
    [UnitSubtitle("Send String to Serial Connection")]
    [UnitCategory("SimpleSerial")]
    [TypeIcon(typeof(Event))]
    public class SerialSendNode : Unit
    {
        [PortLabelHidden, DoNotSerialize] public ControlInput InputTrigger { get; private set; }
        [DoNotSerialize, PortLabelHidden] public ControlOutput OutputTrigger { get; private set; }

        [DoNotSerialize] public ValueInput Connection { get; private set; }
        [DoNotSerialize] public ValueInput Message { get; private set; }

        private SerialConnection serialConnection;

        protected override void Definition()
        {
            InputTrigger = ControlInput(nameof(InputTrigger), UpdateNode);
            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Connection = ValueInput<SerialConnection>(nameof(Connection), null);
            Message = ValueInput<string>(nameof(Message), string.Empty);
        }

        ControlOutput UpdateNode(Flow flow)
        {
            serialConnection.Send(flow.GetValue<string>(Message));
            return OutputTrigger;
        }

        void PortsChanged(Flow flow)
        {
            serialConnection = flow.GetValue<SerialConnection>(Connection);
        }
    }
}