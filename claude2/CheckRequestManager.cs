using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockAutoTrade2
{
    public class CheckRequestManager
    {
        private Queue<Task> TaskQueue = new Queue<Task>(); // Task 큐 생성
        private Thread WorkerThread; // 작업 스레드
        public int delayTime = 260; // 딜레이타임

        public CheckRequestManager()
        {
            WorkerThread = new Thread(delegate ()
            {
                while (true)
                {
                    try
                    {
                        while (TaskQueue.Count > 0)// 큐에 작업할 것이 있으면
                        {
                            Task t;
                            lock (TaskQueue) // TaskQueue를 잠금
                            {
                                t = TaskQueue.Dequeue(); // 큐에서 작업을 꺼냄
                            }
                            t.RunSynchronously(); // 작업을 동기적으로 실행
                            Thread.Sleep(delayTime); // 작업간 딜레이
                        }
                        Thread.Sleep(100); // 감시딜레이
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine("Thread Msg_1: " + e.Message);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine("Thread MSG_2: " + e.Message);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Thread MSG_3: " + exception.Message);
                    }
                }
            });

            WorkerThread.Start(); // 작업 스레드 시작
        }
        public void setDelayTime(int delay)
        {
            delayTime = delay; // 딜레이 타임 설정
        }
        public void Stop()
        {
            WorkerThread.Abort(); // 작업 스레드 중지
        }
        //요청매서드
        public void sendTaskData(Task task)
        {
            lock(TaskQueue) // TaskQueue를 잠금
            {
                TaskQueue.Enqueue(task);// 큐에 작업을 추가
            }
        }
    }
}
