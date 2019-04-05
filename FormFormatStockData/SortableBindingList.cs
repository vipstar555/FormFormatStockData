using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormFormatStockData
{
    //コピペDatagridviewソート用クラス
    public class SortableBindingList<T> : BindingList<T> where T : class
    {
        // ソート済みかどうか
        private bool isSorted;
        /// 並べ替え操作の方向
        /// </summary>
        private ListSortDirection sortDirection = ListSortDirection.Ascending;
        // ソートを行う抽象化プロパティ
        private PropertyDescriptor sortProperty;
        //SortableBindingList クラス の 新しいインスタンス を初期化します。
        public SortableBindingList()
        {
        }
        /// 指定した リストクラス を利用して SortableBindingList クラス の 新しいインスタンス を初期化します。
        public SortableBindingList(IList<T> list) : base(list)
        {
        }
        // リストがソートをサポートしているかどうかを示す値を取得します。
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }
        /// リストがソートされたかどうかを示す値を取得します。
        protected override bool IsSortedCore
        {
            get { return this.isSorted; }
        }
        /// ソートされたリストの並べ替え操作の方向を取得します。
        protected override ListSortDirection SortDirectionCore
        {
            get { return this.sortDirection; }
        }
        /// ソートに利用する抽象化プロパティを取得します。
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return this.sortProperty; }
        }
        /// ApplySortCore で適用されたソートに関する情報を削除します。
        protected override void RemoveSortCore()
        {
            this.sortDirection = ListSortDirection.Ascending;
            this.sortProperty = null;
            this.isSorted = false;
        }
        // 指定されたプロパティおよび方向でソートを行います。
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            // ソートに使う情報を記録
            this.sortProperty = prop;
            this.sortDirection = direction;

            // ソートを行うリストを取得
            var list = Items as List<T>;
            if (list == null)
            {
                return;
            }

            // ソート処理
            list.Sort(this.Compare);

            // ソート完了を記録
            this.isSorted = true;

            // ListChanged イベントを発生させます
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        //比較処理を行います。
        private int Compare(T lhs, T rhs)
        {
            // 比較を行う
            var result = this.OnComparison(lhs, rhs);

            // 昇順の場合 そのまま、降順の場合 反転させる
            return (this.sortDirection == ListSortDirection.Ascending) ? result : -result;
        }
        // 昇順として比較処理を行います。
        private int OnComparison(T lhs, T rhs)
        {
            object lhsValue = (lhs == null) ? null : this.sortProperty.GetValue(lhs);
            object rhsValue = (rhs == null) ? null : this.sortProperty.GetValue(rhs);

            if (lhsValue == null)
            {
                return (rhsValue == null) ? 0 : -1;
            }

            if (rhsValue == null)
            {
                return 1;
            }

            if (lhsValue is IComparable)
            {
                return ((IComparable)lhsValue).CompareTo(rhsValue);
            }

            if (lhsValue.Equals(rhsValue))
            {
                return 0;
            }

            return lhsValue.ToString().CompareTo(rhsValue.ToString());
        }
    }
}
