 using System.Collections.Generic;
 using UnityEngine;

 public abstract class Command
 {
     public abstract void Execute();
     public abstract void Undo();
     public Stack<RotateCommand> affected;
 }