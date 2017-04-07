#include <avr/wdt.h>
int con=0;
int isPress=0;
int inputPin = 4;
int outputPin = 13;

void setup()
{
  wdt_enable(WDTO_4S);
  Serial.begin(38400);
}
        
void loop()
{
  
  if(con==0)
  {
    if(Serial.available()>0)
    {
      Serial.write("check");
      con=1;
      delay(30);
   }
  }
  else if(con==1)
  {
    Serial.println("a");
     
    int clickState = digitalRead(inputPin);
    while(Serial.available() == 0){}
    if(Serial.available()>0)
    {
      String incoming=Serial.readString();
      Serial.println(incoming);
       if(incoming=="skdol")
      { 
        //Serial.println("Reset WDT");
      if (clickState == HIGH)
      {
        if(isPress==0)
        {
          isPress=1;
        Serial.println("isClick");
 
        }
      }
      else 
      {
        if(isPress==1){
          isPress=0;
        }
      }
      
    }
    else
    {
      con=0;
      

      }

    }
  }
  

}
