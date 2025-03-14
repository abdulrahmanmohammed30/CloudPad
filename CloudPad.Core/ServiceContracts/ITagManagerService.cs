﻿using CloudPad.Core.Dtos;

namespace CloudPad.Core.ServiceContracts;

public interface ITagManagerService
{
    Task<TagDto> CreateAsync(int userId, CreateTagDto tag);
    Task<TagDto> UpdateAsync(int userId, UpdateTagDto tag);
    Task<List<TagDto>> UpdateNoteTagsAsync(int userId, Guid noteId, List<int> noteIds);
    Task<bool> DeleteAsync(int userId, int tagId);
    Task DeleteAllAsync(int userId);
}

// Get all tags from database, compare tags and perform insertion and deletion 
// The tags method should be added to the tags service 
// node should exists so that you can add or remove tags 
// the method should throw an exception if user or note doesn't exist 
// then it add the tags 
// What Parameters, What ReturnType 
// AddTagsBulk 
// RemoveTagsBulk 