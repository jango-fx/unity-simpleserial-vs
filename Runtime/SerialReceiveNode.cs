using Unity.VisualScripting;
using UnityEngine;

namespace ƒx.SimpleSerial
{
    [UnitTitle("Serial Event")]
    [UnitCategory("SimpleSerial")]
    [UnitSubtitle("Serial Event")]
    public class SerialReceiveNode : EventUnit<string>
    {
        [DoNotSerialize] public ValueInput Connection { get; private set; }
        [DoNotSerialize] public ValueOutput OutputData { get; private set; }
        private string serialData;
        private SerialConnection serialConnection;

        protected override bool register => true;
        [DoNotSerialize] public static string SerialEvent = "SerialEvent"; // string name to hook EventUnit-event to an script event
        Flow _flow;

        protected override void Definition()
        {
            base.Definition();
            Connection = ValueInput<SerialConnection>(nameof(Connection), null);
            OutputData = ValueOutput<string>(nameof(OutputData), GetSerialData);
        }


        string GetSerialData(Flow flow)
        {
            return serialData;
        }

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(SerialReceiveNode.SerialEvent);
        }

        override public void StartListening(GraphStack stack)
        {
            base.StartListening(stack);

            GraphReference reference = stack.AsReference();
            using var flow = Flow.New(reference);
            serialConnection = flow.GetValue<SerialConnection>(Connection);

            serialConnection.callbackHandler -= OnReceive;
            serialConnection.callbackHandler += OnReceive;
        }

        public void OnReceive(string data){
            serialData = data;
            EventBus.Trigger(SerialReceiveNode.SerialEvent, data);
        }
    }
}