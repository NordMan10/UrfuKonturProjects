using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TodoApplication
{
    public enum Commands
    {
        Add,
        Remove
    }

    public class NotesAndCommands<TItem>
    {
        public List<Tuple<int, TItem>> Notes { get; set { Notes.Add() } }
        public LimitedSizeStack<Commands> RecordedCommands { get; private set; }

        public NotesAndCommands(int limit)
        {
            Notes = new List<Tuple<int, TItem>>();
            RecordedCommands = new LimitedSizeStack<Commands>(limit);
        }

        public void Remove(Tuple<int, TItem> note, Commands command)
        {
            Notes.Add(note);
            RecordedCommands.Push(command);
        }

        public void PushInRCommands(Commands command)
        {
            RecordedCommands.Push(command);
        }

        public Commands PopFromRCommands()
        {
            return RecordedCommands.Pop();
        }
    }

    public class Model<TItem>
    {
        public List<TItem> RecievedItems { get; set; }
        public NotesAndCommands<TItem> NotesAndCommands { get; set; }

        public Model(int limit, List<TItem> items)
        {
            RecievedItems = items;
            NotesAndCommands = new NotesAndCommands<TItem>(limit);
        }

        public void Addition(TItem item)
        {
            RecievedItems.Add(item);
            NotesAndCommands.PushInRCommands(Commands.Add);
        }

        public void Removing(int index)
        {
            NotesAndCommands.Remove(new Tuple<int, TItem>(index, RecievedItems[index]), Commands.Remove);
            RecievedItems.RemoveAt(index);
        }

        public void UnDoing()
        {
            var lastOfItems = RecievedItems[RecievedItems.Count - 1];
            var revert = NotesAndCommands.PopFromRCommands();
            if (revert == Commands.Add) RecievedItems.Remove(lastOfItems);
            else if (revert == Commands.Remove)
            {
                var lastIndex = Notes[Notes.Count - 1].Item1; ;
                var lastNote = Notes[Notes.Count - 1].Item2;
                RecievedItems.Insert(lastIndex, lastNote);
                Notes.RemoveAt(Notes.Count - 1);
            }
        }
    }

    public class ListModel<TItem>
    {
        public List<TItem> Items { get; set; }
        private Model<TItem> model;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            model = new Model<TItem>(limit, Items);
        }
        
        public void AddItem(TItem item)
        {
            model.Addition(item);
        }

        public void RemoveItem(int index)
        {
            model.Removing(index);
        }

        public bool CanUndo()
        {
            return model.RecordedCommands.Count != 0;
        }

        public void Undo()
        {
            model.UnDoing();
        }
    }
}
