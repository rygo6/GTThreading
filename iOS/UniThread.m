#import <mach/mach.h>
#include <pthread.h>

//priority 2 == max, priority 1 == min
void SetIOSThreadPriority(const char * threadName, int priority) {
    NSString *NSThreadName = [[NSString alloc] initWithUTF8String:threadName];
    char ptName[256];
    mach_msg_type_number_t count;
    thread_act_array_t list;
    task_threads(mach_task_self(), &list, &count);
    for (int i = 0; i < count; ++i) {
        pthread_t pt = pthread_from_mach_thread_np(list[i]);
        if (pt) {
            ptName[0] = '\0';
            int rc = pthread_getname_np(pt, ptName, sizeof ptName);
            NSLog(@"mach thread %u: getname returned %d: %s", list[i], rc, ptName);
            NSString *ptNSName = [[NSString alloc] initWithUTF8String:ptName];
            if ([ptNSName containsString:NSThreadName]) {
                NSLog(@"Set priority on Thread %@ %d", NSThreadName, priority);
                int policy;
                struct sched_param param;
                memset(&param, 0, sizeof(struct sched_param));
                pthread_getschedparam(pt, &policy, &param);
                if (priority == 2) {
                    param.sched_priority = sched_get_priority_max(policy);
                } else if (priority == 1) {
                    param.sched_priority = sched_get_priority_min(policy);
                }
                int error = pthread_setschedparam(pt, SCHED_RR, &param);
                NSLog(@"PThread Error return: %d", error);
            }
        } else {
            NSLog(@"mach thread %u: no pthread found", list[i]);
        }
    }
}
