# VirtualTomatoHouse
これは秋田県立大学AICのスマート農業技術の開発プロジェクトで、バーチャル環境でトマト園と収穫ロボを再現し、自動収穫のための機械学習を行うためのものです。

図（概要紹介のための）

<br/>

## インストール
このプロジェクトはUnity(2022.3.2f1)によって起動することができます。
リポジトリをクローンし、VirtualTomatoHouse/Unity/VirtualTomatoHouse を起動してください。

または、ビルド済みのWindowsバイナリをリリースページからダウンロードしてください。

<br/>

## 使用法
### アノテーションの撮影

図（アノテーションシーンの図）

Unity/VirtualTomatoHouse/Assets/Scenes/AutoAnnotation からシーンを起動してください。

ゲームを起動して、Startボタンを押すことでアノテーションの自動撮影が行われます。
VirtualTomatoHouse/Unity/AnnotationImages というフォルダに撮影されたアノテーション画像が保存されます。

「No.」の数字を変えることで、撮影する画像の枚数を変更することができます。

<br/>

### 収穫VR・MR

図（収穫VRの図）

Unity/VirtualTomatoHouse/Assets/Scenes/HarvestTomatoVR からシーンを起動してください。

Oculus Quest2またはOculus Quest3をPCに接続し、Oculus Linkを起動した状態でゲームを起動すると収穫VRを体験することができます。
ハンドトラッキングにより手でトマトを掴むことができます。

Unity/VirtualTomatoHouse/Assets/Scenes/HarvestTomatoMR からMRのシーンを起動することができ、同様の体験をすることができます。

図（収穫MRの図）

<br/>

### 収穫ロボットシミュレータ

図（収穫ロボットシミュレータ）

Unity/VirtualTomatoHouse/Assets/Scenes/HarvestRobot からシーンを起動してください。

ゲームを再生することで、自動収穫ロボットシミュレータが開始されます。

Unity/VirtualTomatoHouse/Assets/Scenes/HarvestRobotVR からVRシーンを起動することができます。

### ライセンス

このプロジェクトは [Apache License 2.0](LICENSE) のもとで公開されています。

© Agri-Innovation Education and Research Center, Akita Prefectural University © PLUSPLUS Co., Ltd.
