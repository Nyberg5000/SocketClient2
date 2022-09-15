using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Client app is the one sending messages to a Server/listener.
// Both listener and client can send messages back and forth once a
// communication is established.
public class SocketClient
{
     static void Main(String[] args)
    {
        StartClient();
        //return 0;
    }

    public static void StartClient()
    {
        byte[] bytes = new byte[1024];

        try
        {
            // Connect to a Remote server
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11111);

            // Create a TCP/IP  socket.
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                // Connect socket to Remote EndPoint by using connect method
                sender.Connect(remoteEP);
                
                //print to screen that we are connected
                Console.WriteLine("Socket connected to -> {0}",
                    sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.
                //creates a message that will be sent to the server. EOF: End Of File
                byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                // Send the data through the socket.
                int bytesSent = sender.Send(msg);

                //data buffer
                byte[] messageReceived = new byte[1024];

                // Receive the response from the remote device.
                int bytesRec = sender.Receive(messageReceived);
                Console.WriteLine("Message from server, Echoed test = {0}",
                    Encoding.ASCII.GetString(messageReceived, 0, bytesRec));

                // Release the socket/close socket using close method
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            //catch socket exceptions
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}