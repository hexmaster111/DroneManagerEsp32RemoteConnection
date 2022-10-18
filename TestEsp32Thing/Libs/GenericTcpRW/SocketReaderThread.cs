using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TestEsp32Thing.Libs.DroneType;
using TestEsp32Thing.Libs.Messages;

namespace TestEsp32Thing.Libs.GenericTcpRW
{
    public class SocketReaderThread
    {
        private readonly Socket _socket;

        public SocketReaderThread(EndPoint endpoint)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _socket.Connect(endpoint);
        }

        public bool IsRunning { get; private set; } = true;


        private object _messageLock = new object();
        private ByteMessageQueue _messageQueue = new ByteMessageQueue();

        public ByteMessageQueue MessageQueue
        {
            get
            {
                ByteMessageQueue result;
                lock (_messageLock)
                {
                    result = _messageQueue;
                }

                return result;
            }
            set
            {
                lock (_messageLock)
                {
                    _messageQueue = value;
                }
            }
        }


        public void WriterThreadFunction()
        {
            Debug.WriteLine("WriterThreadFunction started");

            while (IsRunning)
            {
                //Time to write things
                if (_messageQueue.HasMessages)
                {
                    GenericWriter.Write(_messageQueue.GetNextMessage(out var name), _socket, name);
                }

                Thread.Sleep(10);
            }
        }

        public void ReaderThreadFunction()
        {
            Debug.WriteLine("Reader Thread Started");

            Debug.WriteLine("Sending Initial Connection message");
            GenericWriter.Write(new HandShakeMessage(new DroneId(DroneType.DroneType.Experimental, 5050)), _socket,
                "InitialConnectionHandShake");


            while (IsRunning)
            {
                Thread.Sleep(50);
                var data = new byte[10_000];
                // Read data from socket

                if (_socket.Receive(data) <= 0) continue;
                // Turn the data into a string
                var stringData = Encoding.UTF8.GetString(data, 0, data.Length);
                GenericReaderWriter.RaiseNewDataEvent(stringData);
            }
        }
    }
}

public class ByteMessageQueue
{
    private class ByteMessageQueueItem
    {
        public ByteMessageQueueItem(object data, string name)
        {
            Data = data;
            Name = name;
        }

        public object Data { get; }
        public string Name { get; }
    }

    private ByteMessageQueueItem[] _messages = new ByteMessageQueueItem[0];


    private static object _modifyLock = new object();

    public void AddMessage(object data, string name)
    {
        lock (_modifyLock)
        {
            var newMessages = new ByteMessageQueueItem[_messages.Length + 1];
            Array.Copy(_messages, newMessages, _messages.Length);
            newMessages[_messages.Length] = new ByteMessageQueueItem(data, name);
            _messages = newMessages;
        }
    }

    public object GetNextMessage(out string name)
    {
        lock (_modifyLock)
        {
            var result = _messages[0].Data;
            name = _messages[0].Name;
            var newMessages = new ByteMessageQueueItem[_messages.Length - 1];
            Array.Copy(_messages, 1, newMessages, 0, _messages.Length - 1);
            _messages = newMessages;
            return result;
        }
    }

    public void Clear()
    {
        lock (_modifyLock)
        {
            _messages = new ByteMessageQueueItem[0];
        }
    }

    public bool HasMessages => _messages.Length > 0;

    public int Count => _messages.Length;
}