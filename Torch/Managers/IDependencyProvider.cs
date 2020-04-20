﻿using System;

namespace Torch.Managers
{
    public interface IDependencyProvider
    {
        /// <summary>
        ///     Gets the manager that provides the given type.  If there is no such manager, returns null.
        /// </summary>
        /// <param name="type">Type of manager</param>
        /// <returns>manager, or null if none exists</returns>
        IManager GetManager(Type type);
    }
}