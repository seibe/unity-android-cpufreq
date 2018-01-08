#include <fcntl.h>
#include <stdlib.h>
#include <stdio.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <android/log.h>

#define BUF_SIZE 256
#define LOG_DEBUG_FMT(fmt, ...) (__android_log_print(ANDROID_LOG_DEBUG, "cpufreq", fmt, __VA_ARGS__))
#define LOG_ERROR_FMT(fmt, ...) (__android_log_print(ANDROID_LOG_ERROR, "cpufreq", fmt, __VA_ARGS__))
#define LOG_ERROR(msg) (__android_log_write(ANDROID_LOG_ERROR, "cpufreq", msg))

extern "C" {

int8_t read_freqs(const int8_t in_cpu_num, int32_t *out_cpufreqs)
{
    FILE *fp;
    char filepath[64];
    char buffer[BUF_SIZE];

    if (in_cpu_num < 0) {
        LOG_ERROR("CPU数が0未満です");
        return -1;
    }
    if (out_cpufreqs == nullptr) {
        LOG_ERROR("配列がヌルです");
        return -1;
    }

    for (int8_t i = 0; i < in_cpu_num; i++)
    {
        out_cpufreqs[i] = -1;
        sprintf(filepath, "/sys/devices/system/cpu/cpu%d/cpufreq/scaling_cur_freq", i);

        fp = fopen(filepath, "r");
        if (fp == nullptr) {
            LOG_DEBUG_FMT("オープンに失敗しました (%s)", filepath);
            continue;
        }

        if (fgets(buffer, BUF_SIZE, fp) == nullptr) {
            LOG_ERROR_FMT("読み込みに失敗しました (%d)", i);
        } else {
            LOG_DEBUG_FMT("読み取った値: %s (%d)", buffer, i);
            out_cpufreqs[i] = atoi(buffer);
        }

        if (fclose(fp) != 0) {
            LOG_ERROR_FMT("クローズに失敗しました (%d)", i);
        }
    }
    return 0;
}

}
