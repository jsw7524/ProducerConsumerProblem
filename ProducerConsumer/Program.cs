using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class ProducerConsumer
    {

        private Semaphore semaphoreSpace;

        private Semaphore semaphoreGoods;

        public Stack<int> stack;


        private Mutex mutexProducer;

        private Mutex mutexConsumer;

        private int buffer;

        public ProducerConsumer()
        {
            buffer = 0;
            semaphoreSpace = new Semaphore(10, 10);
            semaphoreGoods = new Semaphore(0, 10);

            mutexProducer = new Mutex(false);
            mutexConsumer = new Mutex(false);
            stack = new Stack<int>(10);
        }

        public async Task Producer()
        {
            await Task.Run(() =>
            {
                while (true) //for (int i=0;i<10;i++)
                {
                    semaphoreSpace.WaitOne();
                    mutexProducer.WaitOne();
                    stack.Push(Thread.CurrentThread.ManagedThreadId);
                    mutexProducer.ReleaseMutex();
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} add 1 to buffer. buffer:{buffer}");
                    semaphoreGoods.Release();

                }
            });

        }
        public async Task Consumer()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    semaphoreGoods.WaitOne();
                    mutexConsumer.WaitOne();
                    var v = stack.Pop();
                    Console.WriteLine(v);
                    mutexConsumer.ReleaseMutex();
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} add 1 to buffer. buffer:{buffer}");
                    semaphoreSpace.Release();
                }

            });
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var pc = new ProducerConsumer();
            for (int i = 1; i <= 10; i++)
            {
                pc.Producer();
            }
            for (int i = 1; i <= 10; i++)
            {
                pc.Consumer();
            }

            var yyy = pc.stack;
            Console.WriteLine("YYYYYYYYYYYYY");
            Console.ReadKey();
        }
    }
}
