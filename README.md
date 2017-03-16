# POJS

a simple Programming Online Judge System
精简C语言判题系统

------

## 配置 configuration

JSON格式

```json
{
    // 服务器监听ip地址
    "ServerIP": "127.0.0.1",
    // 服务器监听端口
    "ServerPort": "7777",
    // 编译器中文件gcc的路径
    "CompilerPath": ".\\POJS\\MinGW\\bin\\gcc.exe",
    // 编译及运行过程文件缓存文件夹位置
    "WorkPath": ".\\",
    // web版本对应资源文件路径
    "ResourcesPath": ".\\Resources\\"
}
```

## 使用 usage

客户端

![client](https://github.com/G-nice/Radar/raw/master/Doc/client.png)

WEB端

![web](https://github.com/G-nice/Radar/raw/master/Doc/web.png)

## 特性 feature

- 支持超时判断
  ​

## 更新日志 Change log

### v0.1.0(2017/03/15)

* 完成基本功能
* 完成界面设计、交互设计
* 超时限制
* WEB版本

## To Do List

- coming