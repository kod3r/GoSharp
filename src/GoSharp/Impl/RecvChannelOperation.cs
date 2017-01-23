﻿using System.Reflection;

namespace GoSharp.Impl
{
    internal sealed class RecvChannelOperation : ChannelOperation
    {
        private readonly MethodInfo _action;

        internal object Msg { get; set; }

        internal RecvChannelOperation(ChannelBase channel, AsyncAutoResetEvent evt, MethodInfo action, object target): base(channel, evt, target)
        {
            _action = action;
        }

        internal RecvChannelOperation(ChannelBase channel, AsyncAutoResetEvent evt) : base(channel, evt, null)
        {
            _action = null;
        }

        internal void ExecuteAction()
        {
            _action.Invoke(_target, new[] {Msg});
        }
    }
}