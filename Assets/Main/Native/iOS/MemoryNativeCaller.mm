#import "mach/mach.h"

extern "C"
{
    int64_t GetUsedMemory()
    {
        struct task_basic_info info;
        mach_msg_type_number_t size = sizeof(info);
        kern_return_t kerr = task_info(mach_task_self(), TASK_BASIC_INFO, (task_info_t)&info, &size);
        return (kerr == KERN_SUCCESS) ? info.resident_size : 0;
    }

    int64_t GetFreeMemory()
    {
        mach_port_t host_port = mach_host_self();
        mach_msg_type_number_t host_size = sizeof(vm_statistics_data_t) / sizeof(integer_t);
        vm_size_t pagesize;
        vm_statistics_data_t vm_stat;
        host_page_size(host_port, &pagesize);
        (void)host_statistics(host_port, HOST_VM_INFO, (host_info_t)&vm_stat, &host_size);
        return vm_stat.free_count * pagesize;
    }
}
