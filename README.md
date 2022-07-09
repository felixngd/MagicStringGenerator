## Tính năng

- 1 click sinh code C# tất cả các key của assets trong các group Addressables
- Tùy chọn tích hợp với Odin Inspector

## Cài đặt

git: [`https://github.com/datndwolffun/AddressablesKeyGenerator.git`](https://github.com/datndwolffun/AddressablesKeyGenerator.git)

chọn bản lớn nhất từ `com.wolffun.codegen.magicstring`

## Dùng như thế nào?

1. Chọn Tools>CodeGen>Addressable Key Generator

![image](https://user-images.githubusercontent.com/105283697/170400970-6e831b60-4297-488e-8b5a-eb08c2fd7d99.png)

1. Cài đặt namespace, class name cho file code được sinh ra và thư mục lưu file đó.
2. Giờ có thể gọi những key này ở bất kỳ đâu trong code của bạn theo cú pháp `ClassName.GroupName.Key` ví dụ`AddressableKey.TexturesAvartar._48Guild`
