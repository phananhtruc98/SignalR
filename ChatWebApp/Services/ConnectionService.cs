﻿using AutoMapper;
using ChatAppAPI.Authorization;
using ChatAppAPI.Data;
using ChatAppAPI.Dtos.Connection;
using ChatAppAPI.Entities;
using ChatAppAPI.Hubs;

namespace ChatAppAPI.Services
{
    public interface IConnectionService
    {
        Task DeleteAsync(string connectionId);
    }
    public class ConnectionService: IConnectionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ConnectionService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task DeleteAsync(string connectionId)
        {
            var connection = _context.Connections.Find(connectionId);
            var user = _context.Users.Find(connection.UserId);
            user.IsOnline = false;
            _context.Update(user);
            _context.Remove(connection);
            _context.SaveChanges();
        }
    }
}
