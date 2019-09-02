#include <ESP8266WiFi.h>

//Network name and password
const char* ssid = "Halis'in Telefonu";
const char* password = "bilmiyorum";

//Full Box Variable Definitions
long durationFull;
int distanceFull;
bool isReadyFull;
int tFull=0;

int echoPinFull = 14; // GPIO14---D5 of NodeMCU
int trigPinFull = 12; // GPIO12---D6 of NodeMCU
int ledPinFull = 13; // GPIO13---D7 of NodeMCU

//Empty Box Variable Definitions
long durationEmpty;
int distanceEmpty;
bool isReadyEmpty;
int tEmpty=0;

int echoPinEmpty = 5; // GPIO5---D1 of NodeMCU
int trigPinEmpty = 4; // GPIO4---D2 of NodeMCU
int ledPinEmpty = 0; // GPIO0---D3 of NodeMCU

WiFiServer server(80);
 
void setup() {

  Serial.begin(115200);
  delay(10);

  pinMode(trigPinFull, OUTPUT); // Sets the trigPinFull as an Output
  pinMode(echoPinFull, INPUT); // Sets the echoPinFull as an Input
  pinMode(ledPinFull, OUTPUT);  // Sets the LED as an Output

  pinMode(trigPinEmpty, OUTPUT); // Sets the trigPinEmpty as an Output
  pinMode(echoPinEmpty, INPUT); // Sets the echoPinEmpty as an Input
  pinMode(ledPinEmpty, OUTPUT);  // Sets the LED as an Output
 
  // Connect to WiFi network
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
 
  WiFi.begin(ssid, password);
 
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
 
  // Start the server
  server.begin();
  Serial.println("Server started");
 
  // Print the IP address
  Serial.print("Use this URL to connect: ");
  Serial.print("http://");
  Serial.print(WiFi.localIP());
  Serial.println("/");
 
}
 
void loop() {
    //Full Box UltraSound Sensor Settings
  // Clears the trigPinFull
  digitalWrite(trigPinFull, LOW);
  delayMicroseconds(2);
  // Sets the trigPinFull on HIGH state for 10 micro seconds
  digitalWrite(trigPinFull, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPinFull, LOW);
  // Reads the echoPinFull, returns the sound wave travel time in microseconds
  durationFull = pulseIn(echoPinFull, HIGH);
  // Calculating the distanceFull
  distanceFull= durationFull*0.034/2;
  // Prints the distanceFull on the Serial Monitor
  Serial.print("DistanceFull: ");
  Serial.println(distanceFull);

    //Empty Box UltraSound Sensor Settings
  // Clears the trigPinEmpty
  digitalWrite(trigPinEmpty, LOW);
  delayMicroseconds(2);
  // Sets the trigPinEmpty on HIGH state for 10 micro seconds
  digitalWrite(trigPinEmpty, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPinEmpty, LOW);
  // Reads the echoPinEmpty, returns the sound wave travel time in microseconds
  durationEmpty = pulseIn(echoPinEmpty, HIGH);
  // Calculating the distanceEmpty
  distanceEmpty= durationEmpty*0.034/2;
  // Prints the distanceEmpty on the Serial Monitor
  Serial.print("DistanceEmpty: ");
  Serial.println(distanceEmpty);

  
    //isReadyFull Condition
  if(distanceFull<10){
  tFull++;
    if(tFull>2000){
      tFull=0;
      isReadyFull=1;
    }
  }
  else
  {
   isReadyFull=0;
  }
  
  //isReadyEmpty Condition
  if(distanceEmpty>10){
  tEmpty++;
    if(tEmpty>2000){
      tEmpty=0;
      isReadyEmpty=1;
    }
  }
  else
  {
    isReadyEmpty=0;
  } 
  
  //Full LED Conditions
  if (isReadyFull==1)  {
    digitalWrite(ledPinFull, HIGH);
  }
  else{
    digitalWrite(ledPinFull, LOW);
  }

  //Empty LED Conditions
  if (isReadyEmpty==1)  {
    digitalWrite(ledPinEmpty, HIGH);
  }
  else{
    digitalWrite(ledPinEmpty, LOW);
  }
  
  // Check if a client has connected
  WiFiClient client = server.available();
  if (!client) {
    return;
  }

  // Read the first line of the request
  String request = client.readStringUntil('\r');
  Serial.println(request);
  client.flush();
  
  // Return the response
  client.println("HTTP/1.1 200 OK");
  client.println("Content-Type: text/html");
  client.println(""); //  do not forget this one
  client.println("<!DOCTYPE HTML>");
  client.println("<html>");
  client.print("<style>");
  client.print("p {font-size:12px;}");
  client.print("</style>");    
  client.print("<head>");
  client.print("<meta http-equiv=\"refresh\" content=\"2;url=/\">");
  client.print("<TITLE />Supply Box App</title>");
  client.print("</head>");

//Full Shipment App Warning
  if(isReadyFull==1){
    client.print("<p style=\"font-size:25px;color:RED;\">");
  }
  else{
    client.print("<p style=\"font-size:25px;\">");
  }
  client.print(" DOLU KUTU"); 
  client.print("<\/p>");

  //Empty Shipment App Warning
  if(isReadyEmpty==1){
    client.print("<p style=\"font-size:25px;color:RED;\">");
  }
  else{
    client.print("<p style=\"font-size:25px;\">");
  }
  client.print(" BOŞ KUTU"); 
  client.print("<\/p>");
  
  if(isReadyEmpty==1){
    client.print("<p style=\"color:RED;\">COROLLA ROOF hatti bos kutu bekliyor!</p>");
  }
  if(isReadyFull==1){
    client.print("<p style=\"color:RED;\">COROLLA ROOF hatti transpalet bekliyor!</p>");
  }

  client.println("</html>");

  Serial.println("");
 
}
