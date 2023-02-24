using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreApi.Context;
using coreApi.Models;

namespace coreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly CoreApiContext _context;

        public TweetsController(CoreApiContext context)
        {
            _context = context;
        }

        // GET: api/Tweets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweets()
        {
          if (_context.Tweets == null)
          {
              return NotFound();
          }
            return await _context.Tweets.ToListAsync();
        }

        // GET: api/Tweets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tweet>> GetTweet(int id)
        {
          if (_context.Tweets == null)
          {
              return NotFound();
          }
            var tweet = await _context.Tweets.FindAsync(id);

            if (tweet == null)
            {
                return NotFound();
            }

            return tweet;
        }

        // PUT: api/Tweets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTweet(int id, Tweet tweet)
        {
            if (id != tweet.Id)
            {
                return BadRequest();
            }

            _context.Entry(tweet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TweetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tweets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tweet>> PostTweet(Tweet tweet)
        {
          if (_context.Tweets == null)
          {
              return Problem("Entity set 'CoreApiContext.Tweets'  is null.");
          }
            _context.Tweets.Add(tweet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTweet", new { id = tweet.Id }, tweet);
        }

        // DELETE: api/Tweets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTweet(int id)
        {
            if (_context.Tweets == null)
            {
                return NotFound();
            }
            var tweet = await _context.Tweets.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }

            _context.Tweets.Remove(tweet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TweetExists(int id)
        {
            return (_context.Tweets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
