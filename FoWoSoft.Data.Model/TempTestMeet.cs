using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.Model
{
  public  class TempTestMeet
    {
       
      public Guid  ID { get; set; }  
      public string  Title { get; set; }
      public string  UserID { get; set; }    
      public string  UserID_text { get; set; }     
      public string  DeptID { get; set; }  
      public string  DeptName { get; set; }  
      public DateTime?  Date1 { get; set; }   
      public DateTime?  Date2 { get; set; }    
      public string  Type { get; set; }   
      public string  Reason { get; set; }  
      public string  WriteTime { get; set; }  
      public string  test { get; set; }   
      public string  test1 { get; set; } 
      public string  abroad { get; set; } 
      public string  inland { get; set; }  
      public string  college { get; set; }    
      public string  test2_text { get; set; }
      public int flowcompleted { get; set; } 
    }
}
