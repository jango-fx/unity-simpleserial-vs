int led = 13;                 // PWM capable LED control pin

String inputString = "";      // String to hold incoming data
bool stringComplete = false;  // whether the string is complete

long lastTime;                // save time for stop watch

void setup() {
  Serial.begin(115200);       // initialize serial:
  inputString.reserve(200);   // reserve 200 bytes for the inputString
  pinMode(led, OUTPUT);       // set pin as output
}


void loop() {
  if (stringComplete) {                     // when a newline arrives…
    Serial.println(inputString);            // print the string
    analogWrite(led, inputString.toInt());  // parse it as integer and use it as PWM value for the LED (assuming it's between 0 and 255)
    
    inputString = "";                       // reset the string
    stringComplete = false;                 // reset the completeness check
  }

  long currentTime = millis();
  long timePassed = currentTime-lastTime;   // compute stop watch
  if(timePassed > 500){                     // if more than 0.5seconds have passed since we last took time…
    Serial.println("Hello?");               // print to Serial
    lastTime = currentTime;                 // and reset the stop watch
  }
}


/*
  SerialEvent occurs whenever a new data comes in the hardware serial RX. This
  routine is run between each time loop() runs, so using delay inside loop can
  delay response. Multiple bytes of data may be available.
*/
void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
  }
}