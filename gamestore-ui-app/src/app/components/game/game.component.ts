import { Component, OnInit, effect } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GameService, Game } from '../../services/game.service';
import { LocalizationService } from '../../services/localization.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { CommentService, Comment } from '../../services/comment.service';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-game',
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './game.component.html',
  styleUrl: './game.component.css'
})
export class GameComponent implements OnInit {
  gameId: string | null = null;
  loading = false;
  error: string | null = null;
  
  game: any = null;
  private componentInitialized = false;
  private languageEffectFirstRun = true;

  selectedImageIndex = 0;
  comments: any[] = [];
  
  // UI-only like storage - resets on page refresh
  private likedComments: Set<number> = new Set();
  private commentLikeCounts: Map<number, number> = new Map();
  
  // Comment form properties
  showCommentForm = false;
  newCommentUsername = '';
  newCommentContent = '';
  newCommentRating = 5;
  hoverRating = 0;
  animatingLike: number | null = null;

  // Available game cover images (only using files without spaces or special characters)
  private gameImages = [
    'assets/photos/Leonardo_Phoenix_10_a_lush_3D_render_of_a_computer_game_cover_1.jpg',
    'assets/photos/Leonardo_Phoenix_10_a_lush_3D_render_of_a_computer_game_cover_3.jpg',
    'assets/photos/Lucid_Origin_generate_a_futuristic_computer_game_cover_photo_w_0.jpg',
    'assets/photos/Lucid_Origin_generate_a_futuristic_computer_game_cover_photo_w_1.jpg',
    'assets/photos/Lucid_Origin_generate_a_futuristic_computer_game_cover_photo_w_2.jpg',
    'assets/photos/Lucid_Origin_generate_a_futuristic_computer_game_cover_photo_w_3.jpg',
    'assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_0.jpg',
    'assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_1.jpg',
    'assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_2.jpg',
    'assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_3.jpg'
  ];

  constructor(
    private route: ActivatedRoute,
    private gameService: GameService,
    private commentService: CommentService,
    private orderService: OrderService,
    private localizationService: LocalizationService
  ) {
    // Watch for language changes and reload game data automatically
    effect(() => {
      const currentLang = this.localizationService.currentLanguage();
      this.handleLanguageChange();
    });
  }

  ngOnInit() {
    this.gameId = this.route.snapshot.paramMap.get('id');
    
    if (this.gameId) {
      // Set initial state and load real data immediately
      this.loading = true;
      this.error = null;
      this.game = null;
      this.tryLoadRealGame(this.gameId);
    } else {
      // Only load mock data if no gameId is provided
      this.loadMockGame();
      // Mark component as initialized for mock data scenario
      this.componentInitialized = true;
    }
  }

  private handleLanguageChange() {
    // Skip the first run (initial effect trigger)
    if (this.languageEffectFirstRun) {
      this.languageEffectFirstRun = false;
      return;
    }
    
    // Only reload if component is initialized, we have gameId, and not currently loading
    if (this.componentInitialized && this.gameId && !this.loading && this.game) {
      this.tryLoadRealGame(this.gameId);
    }
  }

  tryLoadRealGame(id: string) {
    // Set a timeout to ensure we don't wait indefinitely
    const timeoutId = setTimeout(() => {
      this.loading = false;
      this.error = 'Request timed out';
      this.componentInitialized = true;
      this.loadMockGame();
    }, 3000); // 3 second timeout

    this.gameService.getGameById(id).subscribe({
      next: (game) => {
        clearTimeout(timeoutId);
        
        if (game && Object.keys(game).length > 0) {
          // Update the game data and clear loading state
          this.game = { ...game, id: this.gameId };
          this.loading = false;
          this.error = null;
          // Mark component as initialized after successful load
          this.componentInitialized = true;
          // Load comments with real game data
          this.loadComments();
        } else {
          this.loading = false;
          this.componentInitialized = true;
          this.loadMockGame();
        }
      },
      error: (error) => {
        clearTimeout(timeoutId);
        console.error('Failed to load real game from API:', error);
        this.loading = false;
        this.error = 'Failed to load game data';
        this.componentInitialized = true;
        this.loadMockGame();
      }
    });
  }

  private loadMockGame() {
    this.loading = false;
    this.error = null;
    this.game = {
      id: 'mock-game-1',
      key: 'ace-combat-7',
      name: 'Ace Combat 7: Skies Unknown',
      description: 'Become an ace pilot and soar through photorealistic skies with full 360 degree movement; down enemy aircraft and experience the thrill of engaging in realistic sorties! Aerial combat has never looked or felt better!',
      price: 59.99,
      discount: 15,
      rating: 4.5,
      reviewCount: 142,
      genre: 'Action, Adventure',
      publisher: 'BANDAI NAMCO Entertainment America Inc',
      platform: 'PC',
      images: ['1', '2', '3', '4']
    };
    // Load comments after mock game is set
    this.loadComments();
  }

  loadComments() {
    // Ensure game is loaded before trying to access its properties
    if (!this.game) {
      this.comments = this.getMockComments();
      this.initializeLikeCounts();
      return;
    }

    const gameKey = this.game.key || this.gameId || '1';
    
    this.commentService.getCommentsByGame(gameKey).subscribe({
      next: (comments) => {
        this.comments = comments.length > 0 ? comments : this.getMockComments();
        // Initialize like counts from original comment data
        this.initializeLikeCounts();
      },
      error: (error) => {
        console.error('Failed to load comments:', error);
        // Use mock comments if API fails
        this.comments = this.getMockComments();
        this.initializeLikeCounts();
      }
    });
  }

  private getMockComments() {
    return [
      {
        id: 1,
        username: 'Wesley Powell',
        rating: 5,
        date: '2024-01-15',
        likes: 12,
        content: 'The aerial dogfights are the best this generation has, and the additional bosses will help you the memory of Shred of Mercy. Places, factors, for aerial combat immersion should be the truth how sophisticated and realistic should be in gameplay. They make one question what a real world aircraft would deliver in gaming experience.',
        verified: true
      },
      {
        id: 2,
        username: 'Alex Hill',
        rating: 5,
        date: '2024-01-12',
        likes: 8,
        content: 'Ace Combat 7 Skies Unknown is THE entry in the Air Combat Station themed games. The single player campaign starts slow but is engrossing, and the visual and mechanics truly feel you can you play through and over any battle. These games are extremely good - the control very excellent overall and',
        verified: false
      },
      {
        id: 3,
        username: 'George Hernandez',
        rating: 4,
        date: '2024-01-10',
        likes: 15,
        content: 'Outstanding graphics, Recommended by the directions Squad format needed perfect aircraft pilot and control for a truly realistic aircraft. The story narrative gives you a very narrative storyline, and the overall presentation is outstanding along with the graphics.',
        verified: true
      }
    ];
  }

  private initializeLikeCounts() {
    this.commentLikeCounts.clear();
    this.likedComments.clear();
    
    this.comments.forEach(comment => {
      // Store original like count
      this.commentLikeCounts.set(comment.id, comment.likes || 0);
    });
  }

  selectImage(index: number) {
    this.selectedImageIndex = index;
  }

  getStars(rating: number): string {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    return '★'.repeat(fullStars) + (hasHalfStar ? '☆' : '') + '☆'.repeat(5 - fullStars - (hasHalfStar ? 1 : 0));
  }

  getDiscountedPrice(): number {
    if (!this.game || !this.game.price) return 59.99; // Default price
    const discount = this.game.discount || 0;
    return this.game.price - (this.game.price * discount / 100);
  }

  getTax(): number {
    return this.getDiscountedPrice() * 0.08; // 8% tax
  }

  getTotalPrice(): number {
    return this.getDiscountedPrice() + this.getTax();
  }

  likeComment(commentId: number) {
    const isCurrentlyLiked = this.likedComments.has(commentId);
    
    // Add animation effect
    this.animatingLike = commentId;
    setTimeout(() => {
      this.animatingLike = null;
    }, 300);
    
    if (isCurrentlyLiked) {
      // User is un-liking the comment
      this.likedComments.delete(commentId);
      const currentCount = this.commentLikeCounts.get(commentId) || 0;
      this.commentLikeCounts.set(commentId, Math.max(0, currentCount - 1));
    } else {
      // User is liking the comment
      this.likedComments.add(commentId);
      const currentCount = this.commentLikeCounts.get(commentId) || 0;
      this.commentLikeCounts.set(commentId, currentCount + 1);
    }
    
    // Visual feedback
  }

  // Get current like count for a comment (includes UI changes)
  getLikeCount(commentId: number): number {
    return this.commentLikeCounts.get(commentId) || 0;
  }

  // Check if current user has liked this comment
  isCommentLiked(commentId: number): boolean {
    return this.likedComments.has(commentId);
  }

  addToCart() {
    if (!this.game) {
      alert('Game information not available');
      return;
    }

    const gameKey = this.game.key || this.game.id;
    
    if (!gameKey) {
      alert('Unable to add game to cart - missing game identifier');
      return;
    }
    
    this.orderService.addGameToCart(gameKey).subscribe({
      next: (response) => {
        alert(`${this.game.name} has been added to your cart!`);
      },
      error: (err) => {
        console.error('Error adding to cart:', err);
        alert('Failed to add game to cart. Please try again.');
      }
    });
  }

  deleteComment(commentId: number) {
    const gameKey = this.game.key || this.gameId || '1';
    
    this.commentService.deleteComment(gameKey, commentId).subscribe({
      next: () => {
        this.comments = this.comments.filter(c => c.id !== commentId);
      },
      error: (error) => {
        console.error('Failed to delete comment:', error);
        alert('Failed to delete comment');
      }
    });
  }

  getRandomGameImage(gameId?: string): string {
    // Use game ID to ensure consistent image for the same game across renders
    const seed = gameId ? this.hashCode(gameId) : Math.random();
    const index = Math.abs(seed) % this.gameImages.length;
    return this.gameImages[index];
  }

  private hashCode(str: string): number {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      const char = str.charCodeAt(i);
      hash = ((hash << 5) - hash) + char;
      hash = hash & hash; // Convert to 32bit integer
    }
    return hash;
  }

  // Comment form methods
  toggleCommentForm() {
    this.showCommentForm = !this.showCommentForm;
    if (!this.showCommentForm) {
      this.resetCommentForm();
    }
  }

  setRating(rating: number) {
    this.newCommentRating = rating;
  }

  submitComment() {
    if (!this.newCommentUsername.trim() || !this.newCommentContent.trim()) {
      alert('Please fill in all fields');
      return;
    }

    const newComment = {
      id: Math.max(0, ...this.comments.map(c => c.id)) + 1,
      username: this.newCommentUsername.trim(),
      rating: this.newCommentRating,
      date: new Date().toISOString(),
      likes: 0,
      content: this.newCommentContent.trim(),
      verified: false
    };

    // Add comment to the beginning of the list
    this.comments.unshift(newComment);
    
    // Initialize like count for the new comment
    this.commentLikeCounts.set(newComment.id, 0);
    
    // Reset form
    this.resetCommentForm();
    this.showCommentForm = false;
    
    // Show success message
    alert('Your review has been submitted successfully!');
  }

  cancelComment() {
    this.resetCommentForm();
    this.showCommentForm = false;
  }

  private resetCommentForm() {
    this.newCommentUsername = '';
    this.newCommentContent = '';
    this.newCommentRating = 5;
    this.hoverRating = 0;
  }

  formatDate(dateString: string): string {
    if (!dateString) return 'Recent';
    
    const date = new Date(dateString);
    const now = new Date();
    const diffTime = Math.abs(now.getTime() - date.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    
    if (diffDays === 1) return '1 day ago';
    if (diffDays < 7) return `${diffDays} days ago`;
    if (diffDays < 30) return `${Math.ceil(diffDays / 7)} weeks ago`;
    
    return date.toLocaleDateString();
  }

  loadMoreComments() {
    // For demo purposes, we'll duplicate existing comments with new IDs
    const additionalComments = this.getMockComments().map((comment, index) => ({
      ...comment,
      id: Math.max(0, ...this.comments.map(c => c.id)) + index + 1,
      username: comment.username + ' (More)',
      date: new Date(Date.now() - Math.random() * 7 * 24 * 60 * 60 * 1000).toISOString()
    }));
    
    this.comments.push(...additionalComments);
    
    // Initialize like counts for new comments
    additionalComments.forEach(comment => {
      this.commentLikeCounts.set(comment.id, comment.likes || 0);
    });
  }
}
