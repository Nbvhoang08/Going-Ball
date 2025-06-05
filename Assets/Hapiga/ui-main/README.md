# Big Bear's UI package

Package dùng để làm UI cho các project của Big Bear

## Hướng dẫn sử dụng

### cài đặt package

1. Trong Unity mở <b>Package Manager</b> 
2. Chọn <b>Add Package from git URL...</b>
3. Điền url = https://gitlab.com/big-bear-team/packages/package-ui.git

### Điều kiện
Trong project cần có các package sau để có thể dùng đc package UI:
1. <b>Big Bear Core</b> [(link)](https://gitlab.com/big-bear-team/packages/package-core.git)
2. **Dotween**

### Setup trong Scene
1. Tạo **Canvas** mặc định của Unity
2. Tạo UIPanel trong **Canvas** : Chuột phải vào **Canvas** chọn **UI -> UIPanel**

### Các thành phần
1. UIPanel : là panel cơ bản để làm 1 chức năng game, có các tính năng 
 - Animation in, out 
 - Event In out
 - Có sử dụng background hay không

2. Tạo bằng cách : chuột phải vào Canvas, chọn **UI -> UIPanel**
3. 