using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashpointCurator.Utils
{
    static class TreeNodeCollectionUtil
    {
        public enum Filter
        {
            NONE,
            EXCLUDE_PARENTS,
            EXCLUDE_CHILDREN
        }

        // Recursively filters a TreeNodeCollection
        public static IEnumerable<TreeNode> All(this TreeNodeCollection nodes, Filter filter = Filter.NONE)
        {
            var stack = new Stack<TreeNodeCollection>();
            stack.Push(nodes);
            while (stack.Count > 0)
            {
                foreach (TreeNode node in stack.Pop())
                {
                    if (node.Nodes.Count != 0)
                    {
                        stack.Push(node.Nodes);
                        if (filter == Filter.EXCLUDE_PARENTS)
                            continue;
                    }
                    else if (filter == Filter.EXCLUDE_CHILDREN)
                        continue;
                    yield return node;
                }
            }
        }
    }
}
