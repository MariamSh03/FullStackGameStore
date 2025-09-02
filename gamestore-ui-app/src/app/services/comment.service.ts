import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { filter, take, switchMap } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';

export interface Comment {
  id: number;
  username: string;
  rating: number;
  date: string;
  likes: number;
  content: string;
  verified: boolean;
  gameKey?: string;
}

export interface CommentRequest {
  content: string;
  rating: number;
  gameKey: string;
}

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {}

  getCommentsByGame(gameKey: string): Observable<Comment[]> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.commentsByGameApiUrl, { key: gameKey });
        return this.http.get<Comment[]>(url);
      })
    );
  }

  addComment(gameKey: string, comment: CommentRequest): Observable<Comment> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.addCommentApiUrl, { key: gameKey });
        return this.http.post<Comment>(url, comment);
      })
    );
  }

  deleteComment(gameKey: string, commentId: number): Observable<void> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.deleteCommentApiUrl, { key: gameKey, id: commentId });
        return this.http.delete<void>(url);
      })
    );
  }

  getBanDurations(): Observable<any[]> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.getBanDurationsApiUrl);
        return this.http.get<any[]>(url);
      })
    );
  }

  banUser(userId: number, duration: number): Observable<void> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.banUserApiUrl);
        return this.http.post<void>(url, { userId, duration });
      })
    );
  }


}
