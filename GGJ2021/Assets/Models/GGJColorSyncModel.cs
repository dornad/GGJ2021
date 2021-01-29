using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class GGJColorSyncModel {
    
    /// RealtimeProperty has 3 arguments: PropertyID, Reliable, ChangeEvent
    /// - https://normcore.io/documentation/core-concepts/synchronizing-custom-data.html#propertyid
    /// - https://normcore.io/documentation/core-concepts/synchronizing-custom-data.html#reliable-unreliable
    /// - https://normcore.io/documentation/core-concepts/synchronizing-custom-data.html#change-event
    [RealtimeProperty(1, true, true)]
    private Color _color;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class GGJColorSyncModel : RealtimeModel {
    public UnityEngine.Color color {
        get {
            return _cache.LookForValueInCache(_color, entry => entry.colorSet, entry => entry.color);
        }
        set {
            if (this.color == value) return;
            _cache.UpdateLocalCache(entry => { entry.colorSet = true; entry.color = value; return entry; });
            InvalidateReliableLength();
            FireColorDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(GGJColorSyncModel model, T value);
    public event PropertyChangedHandler<UnityEngine.Color> colorDidChange;
    
    private struct LocalCacheEntry {
        public bool colorSet;
        public UnityEngine.Color color;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache = new LocalChangeCache<LocalCacheEntry>();
    
    public enum PropertyID : uint {
        Color = 1,
    }
    
    public GGJColorSyncModel() : this(null) {
    }
    
    public GGJColorSyncModel(RealtimeModel parent) : base(null, parent) {
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        UnsubscribeClearCacheCallback();
    }
    
    private void FireColorDidChange(UnityEngine.Color value) {
        try {
            colorDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        int length = 0;
        if (context.fullModel) {
            FlattenCache();
            length += WriteStream.WriteBytesLength((uint)PropertyID.Color, WriteStream.ColorToBytesLength());
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.colorSet) {
                length += WriteStream.WriteBytesLength((uint)PropertyID.Color, WriteStream.ColorToBytesLength());
            }
        }
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var didWriteProperties = false;
        
        if (context.fullModel) {
            stream.WriteBytes((uint)PropertyID.Color, WriteStream.ColorToBytes(_color));
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.colorSet) {
                _cache.PushLocalCacheToInflight(context.updateID);
                ClearCacheOnStreamCallback(context);
            }
            if (entry.colorSet) {
                stream.WriteBytes((uint)PropertyID.Color, WriteStream.ColorToBytes(entry.color));
                didWriteProperties = true;
            }
            
            if (didWriteProperties) InvalidateReliableLength();
        }
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.Color: {
                    UnityEngine.Color previousValue = _color;
                    _color = ReadStream.ColorFromBytes(stream.ReadBytes());
                    bool colorExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.colorSet);
                    if (!colorExistsInChangeCache && _color != previousValue) {
                        FireColorDidChange(_color);
                    }
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
        }
    }
    
    #region Cache Operations
    
    private StreamEventDispatcher _streamEventDispatcher;
    
    private void FlattenCache() {
        _color = color;
        _cache.Clear();
    }
    
    private void ClearCache(uint updateID) {
        _cache.RemoveUpdateFromInflight(updateID);
    }
    
    private void ClearCacheOnStreamCallback(StreamContext context) {
        if (_streamEventDispatcher != context.dispatcher) {
            UnsubscribeClearCacheCallback(); // unsub from previous dispatcher
        }
        _streamEventDispatcher = context.dispatcher;
        _streamEventDispatcher.AddStreamCallback(context.updateID, ClearCache);
    }
    
    private void UnsubscribeClearCacheCallback() {
        if (_streamEventDispatcher != null) {
            _streamEventDispatcher.RemoveStreamCallback(ClearCache);
            _streamEventDispatcher = null;
        }
    }
    
    #endregion
}
/* ----- End Normal Autogenerated Code ----- */
