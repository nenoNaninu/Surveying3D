# アルゴリズム
アルゴリズムは行列式計算して原点と3点を結ぶ4面体の符号付き体積を各meshのtriangle全てに対して行い、合算する。

wavefrontのobjフォーマットのf(ace)は張る面を設定するが、面を表側から見た時、反時計周りに頂点が回るようなフォーマットになっている。

## 注意
wavefrontの面は3点以上で張れるので、4点以上はサポートしない(ばっさり)

## 座標系
右手座標系を考えた時、面を表側から見た時頂点の回り方が反時計だと正、時計回りだと負になる。(正の軸だけ考えると,反時計周りでxyzをなぞれるので分かりがいい)(あとは行列式の列の入れ替えでx-1されていくのだ)

なのでmeshの面を考慮して3x3の行列式を一気に計算して加算していけば、面積が取れる。左手系の時、そのまま計算すると体積が負になるがそれは最後に絶対値を摂ってあげれば問題ないだろう。

