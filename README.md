# VirtualTomatoHouse
これは秋田県立大学AICのスマート農業技術の開発プロジェクトで、バーチャル環境でトマト園と収穫ロボを再現し、自動収穫のための機械学習を行うためのものです。

<br/>

## インストール
このプロジェクトはUnity(2022.3.2f1)によって起動することができます。
リポジトリをクローンし、VirtualTomatoHouse/Unity/VirtualTomatoHouse を起動してください。


<br/>

## 使用法
### アノテーションの撮影

![image](https://github.com/AIC-APU/VirtualTomatoHouse/assets/126754093/2075b3b4-06a8-4919-b7e1-ab5a591ce5ee)


Unity/VirtualTomatoHouse/Assets/Scenes/AutoAnnotation からシーンを起動してください。

ゲームを起動して、Startボタンを押すことでアノテーションの自動撮影が行われます。
VirtualTomatoHouse/Unity/AnnotationImages というフォルダに撮影されたアノテーション画像が保存されます。

「No.」の数字を変えることで、撮影する画像の枚数を変更することができます。

<br/>

### 収穫VR・MR

![トマト収穫VR](https://github.com/AIC-APU/VirtualTomatoHouse/assets/126754093/2bc4b260-02be-4363-9eb7-00cc9829145a)


Unity/VirtualTomatoHouse/Assets/Scenes/HarvestTomatoVR からシーンを起動してください。

Oculus Quest2またはOculus Quest3をPCに接続し、Oculus Linkを起動した状態でゲームを起動すると収穫VRを体験することができます。
ハンドトラッキングにより手でトマトを掴むことができます。

Unity/VirtualTomatoHouse/Assets/Scenes/HarvestTomatoMR からMRのシーンを起動することができ、同様の体験をすることができます。

![MRトマト収穫](https://github.com/AIC-APU/VirtualTomatoHouse/assets/126754093/12eaa5e5-7ab2-4f68-8ce1-b54f6113d96c)

<br/>

### 収穫ロボットシミュレータ

![image](https://github.com/AIC-APU/VirtualTomatoHouse/assets/126754093/e6542e55-eb68-4f26-b09d-bdeacfd6000d)

Unity/VirtualTomatoHouse/Assets/Scenes/HarvestRobot からシーンを起動してください。

ゲームを再生することで、自動収穫ロボットシミュレータが開始されます。

Unity/VirtualTomatoHouse/Assets/Scenes/HarvestRobotVR からVRシーンを起動することができます。

### ライセンス

このプロジェクトは [Apache License 2.0](LICENSE) のもとで公開されています。

© Agri-Innovation Education and Research Center, Akita Prefectural University © PLUSPLUS Co., Ltd.
