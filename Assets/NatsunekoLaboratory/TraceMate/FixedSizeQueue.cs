// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NatsunekoLaboratory.TraceMate
{
    internal class FixedSizeQueue
    {
        private readonly uint _limit;
        private readonly ConcurrentQueue<string> _queue;


        public FixedSizeQueue(uint limit)
        {
            _limit = limit;
            _queue = new ConcurrentQueue<string>();
        }

        public void Enqueue(string obj)
        {
            _queue.Enqueue(obj);
            while (_queue.Count > _limit && _queue.TryDequeue(out var _)) { }
        }

        public IEnumerable<string> AsList()
        {
            return _queue.ToArray();
        }
    }
}