using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class JobExample : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        DoExample();
    }

    private void DoExample()
    {
        // Allocate Memorys
        NativeArray<float> resultArray = new NativeArray<float>(1, Allocator.TempJob);

        // 1. instantiate
        SimpleJob firstJob = new SimpleJob
        {
            // 2. initialize
            a = 5f,
            arrays = resultArray
        };

        AnotherJob secondJob = new AnotherJob
        {
            arrays = resultArray
        };

        // 3. schedule. Best Utilize, Schedule Early
        JobHandle fhandle = firstJob.Schedule();
        JobHandle shandle = secondJob.Schedule(fhandle);

        // other tasks to run in Main Thread in parallel. Best Utilize, Complete late 
        // Complete 전, Native Variable에 접근할 경우 Error 발생
        
        // fhandle.Complete(); <- shandle이 대신함
        shandle.Complete();

        float resultingValue = resultArray[0];
        Debug.Log("Result = " + resultingValue);
        Debug.Log("job A = " + firstJob.a);

        // reAllocate Memorys
        resultArray.Dispose();
    }

    // 중요 : 데이터는 항상 Native Variable, Container를 통해 반환
    private struct SimpleJob : IJob
    {
        public float a;
        public NativeArray<float> arrays;

        /* 추상 메소드 */
        public void Execute()
        {
            arrays[0] = a;
        }
    }

    private struct AnotherJob : IJob
    {
        public NativeArray<float> arrays;

        public void Execute()
        {
            arrays[0] += 1;
        }
    }
}
