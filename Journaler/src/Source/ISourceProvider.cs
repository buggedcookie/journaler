namespace Journaler.Source;

/*
 * For now imma hard code it in each of those
 */
public interface ISourceProvider { string GetPath(); bool Exists(); }

public interface IContentSourceProvider : ISourceProvider { string GetContent(); }