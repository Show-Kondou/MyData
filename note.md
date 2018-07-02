# ECS (Entity Component System)

## ECSとは
- ソフトウェアアーキテクチャパターンの一つで、継承よりコンポジションを軸に設計する。  
- 膨大な量のオブジェクトを処理するのに適した設計になっている。

## ECSの考え方
ECSでは、以下のパターンで設計します。(Unity内では呼び方が本家と異なるので、カッコ内に表記)
- Entity ... データの入れ物
- ConponentData(=>Component) ... Entityに入れるデータ
- ComponentSyetem(=>System)  ... 処理
- Group ... ComponentSystemが処理するときに扱う、ComponentDataの集まり

## ECSの動作
1. **Entity**に**Conponent**が複数種類格納されている。
2. **System**の**Group**に当てはまる**Entity**を探す。
3. 当てはまった**Entity**の**Component**に処理を反映する。

具体的には、、、   
1. **キャラクター(Entity)** に **位置(Component)** と**体力(Component)**、**方向(Component)** が格納されている。
2. **移動処理(System)** で使う**位置&方向(Group)** を持っているオブジェクトを探す。
3. **キャラクター(Entitiy)** が当てはまったので、**キャラクターの位置(Component)** と**方向(Component)** を取得して**移動処理(System)** をする。

## メモ
- 大人数でECSを使って開発する場合は、どんなコンポーネントを作ったか、システムで使うグループはどのコンポーネントかなど、チーム内共有できていないと、思わぬ動作をしたりするので注意がいると思う。
- 今での考えたから、はずれているので入り込むのが難しい。
- まだ、開発段階なので、使えない機能が多く今現在では、ゲーム制作は厳しい。
- MeshInstanveRendererで描画して、実行を止めたりすると、シーンとゲームウィンドウに描画されない事がある。
- ECSはHierarchyに表示されないので、専用のウィンドウ「Entity Debugger」がある。

## SystemのGroupについて
一つのシステムで複数のグループを持つことができる。これによって、プレイヤーのエンティティとエネミーのエンティティを所得して、処理ができる。
また、グループはマイナスのコンポーネントを設定できるので、コンポーネントをラベルの代わりに使って、除外処理をすることができる。

## 課題
- UnityAPIにECSのコンポーネントのデータを設定する。

## 従来との違い
- 今までは、メモリに不規則に配置されていた。
- 今までのコンポーネントは、余分なデータまでついてきた、(Trandform)
- 並列処理ができるようになった。

## はじめようECS
ECSのプロジェクトの設定が少し面倒なので、メモを載せておきます。
1. バージョン 2018 1.0f 以上のUnityを起動。
2. いつもどおりプロジェクトを制作。
3. 起動したらメニューのEdit>ProjectSettings>Playerを選択
4. 開いたInspector内のConfiguration項目のScriptingRuntimeVersionを「.Net 4.X Equivalent」に変更。
5. リロード確認ウィンドウが表示されるので、リロードする。
6. Unityを離れ、プロジェクトフォルダがあるところに移動。
7. プロジェクトフォルダ内に「Packages」フォルダがあるので、その中を開く。
8. 「manifest.json」ファイルがあるので、テキストエディターで開く。
9. 中身は、[ここ](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/TwoStickShooter/Pure/Packages/manifest.json)をコピペして保存。
10. Unityに戻ると、パケージを自動でインポートしてくれる。
	1. 社内ネットワークだとパッケージのリポジトリにアクセス出来ないため、工夫して下さい。
		1. 自分はパッケージ事持ってきました。
11. 終了。

こんな感じで設定します。