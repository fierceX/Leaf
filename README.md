# 掌上题库
## 编译工具： vs2017
其实当初使用的 vs2015，但是2017出来了就升上去了
## 所使用类库 
1. Microsoft.ApplicationInsights: 2.2.0
2. Microsoft.ApplicationInsights.PersistenceChannel: 1.2.3
3. Microsoft.ApplicationInsights.WindowsApps: 1.1.1
4. Microsoft.EntityFrameworkCore.Sqlite: 1.1.1
5. Microsoft.EntityFrameworkCore.Tools: 1.1.0
6. Microsoft.Net.Compilers: 1.3.2
7. Microsoft.NETCore.UniversalWindowsPlatform: 5.3.1
8. MvvmLightLibs: 5.3.0
9. Newtonsoft.Json: 9.0.1
## 开发历程
初次开发手机端app，还是UWP形式的，所有有些问题在所难免，整体采用mvvm框架，但是对mvvm理解的并不深，只是套用了别人的框架，但在开发过程中也受益匪浅，随着开发的进行，对一些模式也有了新的认识。  
第一次开发的时候是为了写软件工程大作业，和一个朋友商量了一下，就开工了，对什么都不了解，套用网上的代码，边学边改边写，总算马马虎虎的完成了大作业。  
后来，大四，为了完善这个项目，也为了给自己点压迫力，答应送同学当毕业设计，然后就修改了一些东西，主要牵扯到sqlite的一些东西，从当初pcl的访问方式，彻底改成了ef的方式，中间也走了一些坑，包括core的ef不支持多对多关系，迂回曲折的总算解决了，还修改了题库导入方式，从单个json文件变成了zip压缩包。  
原来还想上架Windows应用商店，但是没有精力也就不了了之了，以后有新的想法还会继续修改，现在就先到这吧。  
最后，代码太渣不入大佬法眼  
## 题库导入例子
项目里的 **examples.zip** 就是题库例子，里面包含一个 **data.json** 的题库内容文件，和一堆图片 data.json 的格式如下：  

            {
            "Single": [
                {
                    "Image": "图片名",
                    "Stems": "题干",
                    "Answer": "答案",
                    "choices": [
                        "选项1",
                        "选项2",
                        "选项3"
                    ],
                    "Level": 1,
                    "Type": "类型",
                    "Subject": "主题"
                }
            ],
            "Gap": [
                {
                    "Image": "图片名",
                    "Stems": "题干",
                    "Answer": "答案",
                    "Level": 1,
                    "Type": "类型",
                    "Subject": "主题"
                }
            ]
        }

具体看文件里的例子。
## 参考资料
[MVVM之MVVMLight，一个登录注销过程的简单模拟 ](http://www.cnblogs.com/cjw1115/p/5060652.html)  
[win10 在应用中使用SQLite](http://www.cnblogs.com/h82258652/p/4802076.html)  
[关于MVVMLight设计模式系列](http://www.wxzzz.com/958.html)  
[微软官方文档，UWP使用EF](https://docs.microsoft.com/en-us/ef/core/get-started/uwp/getting-started)  
[Win10 UWP 开发系列：使用SQLite](http://www.cnblogs.com/yanxiaodi/p/4941312.html)